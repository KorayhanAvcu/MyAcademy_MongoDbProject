using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Travel.Web.Entities.Tour;
using Travel.Web.Settings;

namespace Travel.Web.Services.TourServices
{
    public class TourService : ITourService
    {
        private readonly IMongoCollection<Tour> _tourCollection;

        public TourService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(
                databaseSettings.Value.ConnectionString);

            var database = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _tourCollection = database.GetCollection<Tour>("Tours");
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

