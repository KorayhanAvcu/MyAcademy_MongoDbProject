using System.Globalization;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using Travel.Web.DTOs.StatisticDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.Enums;
using Travel.Web.Entities.Tour;
using Travel.Web.Settings;

namespace Travel.Web.Services.StatisticServices
{
    /// <summary>
    /// Dashboard istatistikleri. Gruplama gerektiren hesaplamalar
    /// Find() yerine MongoDB Aggregation Pipeline ile yapılır.
    /// </summary>
    public class StatisticService : IStatisticService
    {
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMongoCollection<Reservation> _reservationCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMongoCollection<Destination> _destinationCollection;
        private readonly IMongoCollection<Question> _questionCollection;
        private readonly IMongoCollection<Review> _reviewCollection;
        private readonly UserManager<AppUser> _userManager;

        public StatisticService(
            IDatabaseSettings settings,
            UserManager<AppUser> userManager)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _tourCollection = database.GetCollection<Tour>(settings.TourCollectionName);
            _reservationCollection = database.GetCollection<Reservation>(settings.ReservationCollectionName);
            _categoryCollection = database.GetCollection<Category>(settings.CategoryCollectionName);
            _destinationCollection = database.GetCollection<Destination>(settings.DestinationCollectionName);
            _questionCollection = database.GetCollection<Question>(settings.QuestionCollectionName);
            _reviewCollection = database.GetCollection<Review>(settings.ReviewCollectionName);
            _userManager = userManager;
        }

        public async Task<(int Tours, int Destinations, int Users)> GetSiteCountersAsync()
        {
            var tours = await _tourCollection.CountDocumentsAsync(x => x.Status == TourStatus.Active);
            var destinations = await _destinationCollection.CountDocumentsAsync(x => x.IsActive);
            var users = _userManager.Users.Count();

            return ((int)tours, (int)destinations, users);
        }

        public async Task<DashboardDto> GetDashboardAsync()
        {
            var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);

            var dto = new DashboardDto
            {
                TotalTours = (int)await _tourCollection.CountDocumentsAsync(FilterDefinition<Tour>.Empty),
                ActiveTours = (int)await _tourCollection.CountDocumentsAsync(x => x.Status == TourStatus.Active),
                PassiveTours = (int)await _tourCollection.CountDocumentsAsync(x => x.Status != TourStatus.Active),
                TotalReservations = (int)await _reservationCollection.CountDocumentsAsync(FilterDefinition<Reservation>.Empty),
                ThisMonthReservations = (int)await _reservationCollection.CountDocumentsAsync(x => x.CreatedAt >= monthStart),
                TotalDestinations = (int)await _destinationCollection.CountDocumentsAsync(FilterDefinition<Destination>.Empty),
                PendingQuestions = (int)await _questionCollection.CountDocumentsAsync(x => !x.IsAnswered),
                TotalUsers = _userManager.Users.Count()
            };

            var tourNames = (await _tourCollection.Find(FilterDefinition<Tour>.Empty)
                    .Project(x => new { x.Id, x.Name, x.CoverImageUrl, x.CategoryId, x.DestinationId })
                    .ToListAsync())
                .ToDictionary(x => x.Id);

            // 1) En çok rezervasyon alan 5 tur (iptaller hariç)
            var topTours = await _reservationCollection.Aggregate()
                .Match(x => x.Status != ReservationStatus.Cancelled)
                .Group(x => x.TourId, g => new { TourId = g.Key, Count = g.Count() })
                .SortByDescending(x => x.Count)
                .Limit(5)
                .ToListAsync();

            dto.TopTours = topTours.Select(x => new NameValueDto
            {
                Name = tourNames.TryGetValue(x.TourId, out var t) ? t.Name : "(Silinmiş tur)",
                ImageUrl = tourNames.TryGetValue(x.TourId, out var t2) ? t2.CoverImageUrl : null,
                Value = x.Count
            }).ToList();

            dto.MostReservedTour = dto.TopTours.FirstOrDefault();

            // 2) En yüksek gelir sağlayan tur
            var topRevenue = await _reservationCollection.Aggregate()
                .Match(x => x.Status != ReservationStatus.Cancelled)
                .Group(x => x.TourId, g => new { TourId = g.Key, Revenue = g.Sum(x => x.TotalPrice) })
                .SortByDescending(x => x.Revenue)
                .Limit(1)
                .FirstOrDefaultAsync();

            if (topRevenue != null)
            {
                dto.TopRevenueTour = new NameValueDto
                {
                    Name = tourNames.TryGetValue(topRevenue.TourId, out var t) ? t.Name : "(Silinmiş tur)",
                    Value = (double)topRevenue.Revenue
                };
            }

            // 3) Kategori bazında tur sayısı
            var categories = (await _categoryCollection.Find(FilterDefinition<Category>.Empty).ToListAsync())
                .ToDictionary(x => x.Id, x => x.Name);

            var byCategory = await _tourCollection.Aggregate()
                .Group(x => x.CategoryId, g => new { CategoryId = g.Key, Count = g.Count() })
                .SortByDescending(x => x.Count)
                .ToListAsync();

            dto.ToursByCategory = byCategory.Select(x => new NameValueDto
            {
                Name = x.CategoryId != null && categories.TryGetValue(x.CategoryId, out var n) ? n : "Diğer",
                Value = x.Count
            }).ToList();

            // 4) Destinasyon bazında rezervasyon sayısı ($lookup ile tur dokümanına bağlanır)
            var destinations = (await _destinationCollection.Find(FilterDefinition<Destination>.Empty).ToListAsync())
                .ToDictionary(x => x.Id, x => string.IsNullOrWhiteSpace(x.City) ? x.Country : $"{x.City}, {x.Country}");

            var byDestinationPipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument("Status", new BsonDocument("$ne", (int)ReservationStatus.Cancelled))),
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", _tourCollection.CollectionNamespace.CollectionName },
                    { "localField", "TourId" },
                    { "foreignField", "_id" },
                    { "as", "tour" }
                }),
                new BsonDocument("$unwind", "$tour"),
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", "$tour.DestinationId" },
                    { "count", new BsonDocument("$sum", 1) }
                }),
                new BsonDocument("$sort", new BsonDocument("count", -1))
            };

            var byDestination = await _reservationCollection
                .Aggregate<BsonDocument>(byDestinationPipeline)
                .ToListAsync();

            dto.ReservationsByDestination = byDestination.Select(x =>
            {
                var id = x["_id"].IsBsonNull ? "" : x["_id"].ToString()!;
                return new NameValueDto
                {
                    Name = destinations.TryGetValue(id, out var n) ? n : "Diğer",
                    Value = x["count"].ToInt32()
                };
            }).ToList();

            // 5) Son 6 ayın aylık rezervasyon sayıları
            var sixMonthsAgo = monthStart.AddMonths(-5);

            var monthly = await _reservationCollection.Aggregate()
                .Match(x => x.CreatedAt >= sixMonthsAgo)
                .Group(x => new { x.CreatedAt!.Value.Year, x.CreatedAt!.Value.Month },
                    g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
                .ToListAsync();

            var culture = CultureInfo.CurrentUICulture;

            for (var i = 0; i < 6; i++)
            {
                var month = sixMonthsAgo.AddMonths(i);
                var count = monthly.FirstOrDefault(x => x.Year == month.Year && x.Month == month.Month)?.Count ?? 0;

                dto.MonthlyReservations.Add(new NameValueDto
                {
                    Name = culture.DateTimeFormat.GetMonthName(month.Month),
                    Value = count
                });
            }

            // 6) Ortalama tur puanı
            var avg = await _reviewCollection.Aggregate()
                .Match(x => x.IsActive)
                .Group(x => 1, g => new { Average = g.Average(x => x.Rating) })
                .FirstOrDefaultAsync();

            dto.AverageTourRating = avg == null ? 0 : Math.Round(avg.Average, 1);

            return dto;
        }
    }
}
