using AutoMapper;
using Microsoft.Extensions.Options;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.Enums;
using Travel.Web.Helpers;
using MongoDB.Driver;
using Travel.Web.Entities.Tour;
using Travel.Web.Settings;

namespace Travel.Web.Services.TourServices
{
    public class TourService : ITourService
    {
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMongoCollection<Destination> _destinationCollection;
        private readonly IMapper _mapper;

        public TourService(IOptions<DatabaseSettings> databaseSettings, IMapper mapper)
        {
            var mongoClient = new MongoClient(
                databaseSettings.Value.ConnectionString);

            var database = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _tourCollection = database.GetCollection<Tour>(
                databaseSettings.Value.TourCollectionName);

            _categoryCollection = database.GetCollection<Category>(
                databaseSettings.Value.CategoryCollectionName);

            _destinationCollection = database.GetCollection<Destination>(
                databaseSettings.Value.DestinationCollectionName);

            _mapper = mapper;
        }

        public async Task SetStatusAsync(string id, TourStatus status)
        {
            await _tourCollection.UpdateOneAsync(
                x => x.Id == id,
                Builders<Tour>.Update
                    .Set(x => x.Status, status)
                    .Set(x => x.UpdatedAt, DateTime.UtcNow));
        }

        public async Task<List<TourListItemDto>> GetListItemsAsync(bool onlyActive, IEnumerable<string>? ids = null)
        {
            var filter = Builders<Tour>.Filter.Empty;

            if (onlyActive)
                filter &= Builders<Tour>.Filter.Eq(x => x.Status, TourStatus.Active);

            if (ids != null)
            {
                var idList = ids.ToList();
                filter &= Builders<Tour>.Filter.In(x => x.Id, idList);
            }

            var tours = (await _tourCollection.Find(filter)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync())
                .Localize();

            return await ToListItemsAsync(tours);
        }

        public async Task<List<TourListItemDto>> GetPopularAsync(int count)
        {
            var tours = (await _tourCollection
                .Find(x => x.Status == TourStatus.Active)
                .SortByDescending(x => x.ReservationCount)
                .ThenByDescending(x => x.AverageRating)
                .Limit(count)
                .ToListAsync())
                .Localize();

            return await ToListItemsAsync(tours);
        }

        public async Task<(string? CategoryName, string? Country, string? City)> GetLookupNamesAsync(
            string categoryId, string destinationId)
        {
            var category = await _categoryCollection
                .Find(x => x.Id == categoryId)
                .FirstOrDefaultAsync();

            var destination = await _destinationCollection
                .Find(x => x.Id == destinationId)
                .FirstOrDefaultAsync();

            return (category?.Name, destination?.Country, destination?.City);
        }

        private async Task<List<TourListItemDto>> ToListItemsAsync(List<Tour> tours)
        {
            var categories = (await _categoryCollection.Find(_ => true).ToListAsync())
                .ToDictionary(x => x.Id, x => x.Name);

            var destinations = (await _destinationCollection.Find(_ => true).ToListAsync())
                .ToDictionary(x => x.Id, x => x.Country);

            var result = new List<TourListItemDto>();

            foreach (var tour in tours)
            {
                var dto = _mapper.Map<TourListItemDto>(tour);

                dto.CategoryName = tour.CategoryId != null && categories.TryGetValue(tour.CategoryId, out var c) ? c : null;
                dto.DestinationName = tour.DestinationId != null && destinations.TryGetValue(tour.DestinationId, out var d) ? d : null;

                var upcoming = tour.TourDates
                    .Where(x => x.StartDate.Date >= DateTime.Today && x.Status == TourDateStatus.Active)
                    .OrderBy(x => x.StartDate)
                    .ToList();

                dto.UpcomingDate = upcoming.Select(x => (DateTime?)x.StartDate).FirstOrDefault();
                dto.DateKeys = upcoming.Select(x => $"{x.StartDate:yyyy-MM-dd}|{x.RemainingCapacity}").ToList();
                dto.RemainingCapacity = upcoming.Sum(x => x.RemainingCapacity);
                dto.TotalCapacity = tour.TourDates.Sum(x => x.Capacity);

                result.Add(dto);
            }

            return result;
        }

        public async Task<List<Tour>> GetAllAsync()
        {
            return await _tourCollection
                .Find(_ => true)
                .ToListAsync();
        }

        public async Task<Tour?> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return await _tourCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Tour tour)
        {
            if (tour == null)
                throw new ArgumentNullException(nameof(tour));

            // Yeni tur tarihleri oluşturulurken
            // kalan kapasiteyi başlangıç kapasitesine eşitle.
            foreach (var date in tour.TourDates)
            {
                date.RemainingCapacity = date.Capacity;
            }

            // Yeni oluşturulan turda istatistikler 0'dan başlar.
            tour.AverageRating = 0;
            tour.ReviewCount = 0;
            tour.ReservationCount = 0;

            await _tourCollection.InsertOneAsync(tour);
        }

        public async Task UpdateAsync(Tour tour)
        {
            if (tour == null)
                throw new ArgumentNullException(nameof(tour));

            var existingTour = await GetByIdAsync(tour.Id);

            if (existingTour == null)
                throw new KeyNotFoundException(
                    $"Tour bulunamadı. Id: {tour.Id}");

            /*
             * İstatistikleri koruyoruz.
             * Admin turu güncellerken rating / review /
             * reservation bilgileri sıfırlanmamalı.
             */
            tour.AverageRating = existingTour.AverageRating;
            tour.ReviewCount = existingTour.ReviewCount;
            tour.ReservationCount = existingTour.ReservationCount;

            /*
             * Mevcut TourDate kayıtlarının
             * RemainingCapacity ve Status bilgilerini koruyoruz.
             */
            foreach (var newDate in tour.TourDates)
            {
                if (string.IsNullOrWhiteSpace(newDate.Id))
                {
                    newDate.RemainingCapacity = newDate.Capacity;
                    continue;
                }

                var existingDate = existingTour.TourDates
                    .FirstOrDefault(x => x.Id == newDate.Id);

                if (existingDate != null)
                {
                    newDate.RemainingCapacity =
                        existingDate.RemainingCapacity;

                    newDate.Status = existingDate.Status;
                }
                else
                {
                    newDate.RemainingCapacity = newDate.Capacity;
                }
            }

            await _tourCollection.ReplaceOneAsync(
                x => x.Id == tour.Id,
                tour);
        }

        public async Task DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return;

            await _tourCollection.DeleteOneAsync(
                x => x.Id == id);
        }
    }
}

