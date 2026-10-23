using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.Enums;
using Travel.Web.Entities.Tour;
using Travel.Web.Helpers;
using Travel.Web.Settings;

namespace Travel.Web.Services.ReservationServices
{
    public class ReservationService : IReservationService
    {
        // Çocuk fiyatı yetişkin fiyatının %60'ı (%40 indirim)
        public const decimal ChildPriceRate = 0.6m;

        private readonly IMongoCollection<Reservation> _reservationCollection;
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly UserManager<AppUser> _userManager;

        public ReservationService(
            IDatabaseSettings databaseSettings,
            UserManager<AppUser> userManager)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _reservationCollection = database.GetCollection<Reservation>(
                databaseSettings.ReservationCollectionName);

            _tourCollection = database.GetCollection<Tour>(
                databaseSettings.TourCollectionName);

            _userManager = userManager;
        }

        public decimal CalculateTotal(decimal unitPrice, int adult, int child)
        {
            return adult * unitPrice + child * Math.Round(unitPrice * ChildPriceRate);
        }

        public async Task<(bool Success, string? Error, string? ReservationId)> CreateAsync(
            string userId, CreateReservationDto dto)
        {
            var tour = await _tourCollection
                .Find(x => x.Id == dto.TourId)
                .FirstOrDefaultAsync();

            if (tour == null || tour.Status != TourStatus.Active)
                return (false, "Tur bulunamadı veya satışta değil.", null);

            var tourDate = tour.TourDates.FirstOrDefault(x => x.Id == dto.TourDateId);

            if (tourDate == null || tourDate.Status != TourDateStatus.Active)
                return (false, "Seçilen tur tarihi bulunamadı.", null);

            if (tourDate.StartDate.Date < DateTime.Today)
                return (false, "Geçmiş bir tarihe rezervasyon yapılamaz.", null);

            var totalPerson = dto.AdultCount + dto.ChildCount;

            if (tourDate.RemainingCapacity <= 0)
                return (false, "Bu tarih için kontenjan dolmuştur.", null);

            if (tourDate.RemainingCapacity < totalPerson)
                return (false, $"Bu tarih için yalnızca {tourDate.RemainingCapacity} kişilik kontenjan kaldı.", null);

            // Kontenjanı atomik olarak düş: başka bir istek araya girip
            // kontenjanı tüketirse filtre eşleşmez ve rezervasyon oluşturulmaz.
            var filter = Builders<Tour>.Filter.Eq(x => x.Id, tour.Id)
                & DateElemMatch(tourDate.Id, totalPerson);

            var update = Builders<Tour>.Update
                .Inc("TourDates.$.RemainingCapacity", -totalPerson)
                .Inc(x => x.ReservationCount, 1);

            var result = await _tourCollection.UpdateOneAsync(filter, update);

            if (result.ModifiedCount == 0)
                return (false, "Kontenjan yetersiz. Lütfen kişi sayısını azaltın veya başka tarih seçin.", null);

            var reservation = new Reservation
            {
                UserId = userId,
                TourId = tour.Id,
                TourDateId = tourDate.Id,
                TourStartDate = tourDate.StartDate,
                AdultCount = dto.AdultCount,
                ChildCount = dto.ChildCount,
                TotalPerson = totalPerson,
                TotalPrice = CalculateTotal(tour.Price, dto.AdultCount, dto.ChildCount),
                Status = ReservationStatus.Pending
            };

            await _reservationCollection.InsertOneAsync(reservation);

            return (true, null, reservation.Id);
        }

        public async Task<List<ReservationListDto>> GetAllAsync(ReservationFilterDto? filter = null)
        {
            var fb = Builders<Reservation>.Filter;
            var mongoFilter = fb.Empty;

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.TourId))
                    mongoFilter &= fb.Eq(x => x.TourId, filter.TourId);

                if (!string.IsNullOrWhiteSpace(filter.TourDateId))
                    mongoFilter &= fb.Eq(x => x.TourDateId, filter.TourDateId);

                if (!string.IsNullOrWhiteSpace(filter.UserId))
                    mongoFilter &= fb.Eq(x => x.UserId, filter.UserId);

                if (!string.IsNullOrWhiteSpace(filter.Status)
                    && Enum.TryParse<ReservationStatus>(filter.Status, out var status))
                    mongoFilter &= fb.Eq(x => x.Status, status);

                if (filter.StartDate.HasValue)
                    mongoFilter &= fb.Gte(x => x.TourStartDate, filter.StartDate.Value.Date);

                if (filter.EndDate.HasValue)
                    mongoFilter &= fb.Lt(x => x.TourStartDate, filter.EndDate.Value.Date.AddDays(1));
            }

            var reservations = await _reservationCollection
                .Find(mongoFilter)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();

            return await ToDtosAsync(reservations);
        }

        public async Task<List<ReservationListDto>> GetByUserAsync(string userId)
        {
            return await GetAllAsync(new ReservationFilterDto { UserId = userId });
        }

        public async Task<(bool Success, string? Error)> UpdateStatusAsync(string id, ReservationStatus status)
        {
            var reservation = await _reservationCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (reservation == null)
                return (false, "Rezervasyon bulunamadı.");

            if (reservation.Status == status)
                return (true, null);

            if (status == ReservationStatus.Cancelled)
            {
                // İptal edilen rezervasyonun kontenjanı iade edilir.
                await ChangeCapacityAsync(reservation, reservation.TotalPerson);
            }
            else if (reservation.Status == ReservationStatus.Cancelled)
            {
                // İptal edilmiş rezervasyon yeniden açılırsa kontenjan tekrar düşülür.
                var ok = await ChangeCapacityAsync(reservation, -reservation.TotalPerson);

                if (!ok)
                    return (false, "Bu tarih için yeterli kontenjan kalmadığından rezervasyon yeniden açılamaz.");
            }

            await _reservationCollection.UpdateOneAsync(
                x => x.Id == id,
                Builders<Reservation>.Update
                    .Set(x => x.Status, status)
                    .Set(x => x.UpdatedAt, DateTime.UtcNow));

            return (true, null);
        }

        public async Task<(bool Success, string? Error)> CancelByUserAsync(string id, string userId)
        {
            var reservation = await _reservationCollection
                .Find(x => x.Id == id && x.UserId == userId)
                .FirstOrDefaultAsync();

            if (reservation == null)
                return (false, "Rezervasyon bulunamadı.");

            if (reservation.TourStartDate.Date <= DateTime.Today)
                return (false, "Başlamış veya geçmiş turlar iptal edilemez.");

            return await UpdateStatusAsync(id, ReservationStatus.Cancelled);
        }

        private async Task<bool> ChangeCapacityAsync(Reservation reservation, int delta)
        {
            var tourFilter = Builders<Tour>.Filter.Eq(x => x.Id, reservation.TourId);

            // Kontenjan düşülürken yeterli yer olduğu sorgu seviyesinde kontrol edilir.
            var filter = tourFilter & DateElemMatch(reservation.TourDateId, delta < 0 ? -delta : null);

            var result = await _tourCollection.UpdateOneAsync(
                filter,
                Builders<Tour>.Update.Inc("TourDates.$.RemainingCapacity", delta));

            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Tur tarihini kimliğine göre eşleştirir. Eski kayıtlarda kimlik "_id" yerine
        /// "Id" alanında (ObjectId ya da string) tutulduğu için hepsi kontrol edilir.
        /// </summary>
        private static FilterDefinition<Tour> DateElemMatch(string tourDateId, int? minRemaining)
        {
            var idValues = new BsonArray { tourDateId };
            if (ObjectId.TryParse(tourDateId, out var oid))
                idValues.Add(oid);

            var match = new BsonDocument("$or", new BsonArray
            {
                new BsonDocument("_id", new BsonDocument("$in", idValues)),
                new BsonDocument("Id", new BsonDocument("$in", idValues))
            });

            if (minRemaining.HasValue)
                match.Add("RemainingCapacity", new BsonDocument("$gte", minRemaining.Value));

            return new BsonDocument("TourDates", new BsonDocument("$elemMatch", match));
        }

        private async Task<List<ReservationListDto>> ToDtosAsync(List<Reservation> reservations)
        {
            var tourIds = reservations.Select(x => x.TourId).Distinct().ToList();

            var tours = (await _tourCollection
                .Find(x => tourIds.Contains(x.Id))
                .ToListAsync())
                .Localize()
                .ToDictionary(x => x.Id);

            var users = new Dictionary<string, AppUser?>();

            foreach (var userId in reservations.Select(x => x.UserId).Distinct())
                users[userId] = await _userManager.FindByIdAsync(userId);

            return reservations.Select(r =>
            {
                tours.TryGetValue(r.TourId, out var tour);
                users.TryGetValue(r.UserId, out var user);

                return new ReservationListDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    FullName = user == null ? "-" : $"{user.FirstName} {user.LastName}".Trim(),
                    Email = user?.Email ?? "-",
                    Phone = user?.PhoneNumber ?? "-",
                    TourId = r.TourId,
                    TourName = tour?.Name ?? "(Silinmiş tur)",
                    TourImage = tour?.CoverImageUrl,
                    TourDateId = r.TourDateId,
                    TourStartDate = r.TourStartDate,
                    TourEndDate = tour?.TourDates.FirstOrDefault(d => d.Id == r.TourDateId)?.EndDate,
                    AdultCount = r.AdultCount,
                    ChildCount = r.ChildCount,
                    TotalPerson = r.TotalPerson,
                    TotalPrice = r.TotalPrice,
                    Status = r.Status.ToString(),
                    CreatedAt = r.CreatedAt
                };
            }).ToList();
        }
    }
}
