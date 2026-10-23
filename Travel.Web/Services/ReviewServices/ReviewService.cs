using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Travel.Web.DTOs.ReviewDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.Tour;
using Travel.Web.Settings;

namespace Travel.Web.Services.ReviewServices
{
    public class ReviewService : IReviewService
    {
        private readonly IMongoCollection<Review> _reviewCollection;
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMapper _mapper;

        public ReviewService(
            IDatabaseSettings databaseSettings,
            IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _reviewCollection = database.GetCollection<Review>(
                databaseSettings.ReviewCollectionName);

            _tourCollection = database.GetCollection<Tour>(
                databaseSettings.TourCollectionName);

            _mapper = mapper;
        }

        public async Task<List<ReviewListDto>> GetAllAsync()
        {
            var reviews = await _reviewCollection
                .AsQueryable()
                .ToListAsync();

            var result = _mapper.Map<List<ReviewListDto>>(reviews);

            foreach (var review in result)
            {
                var tour = await _tourCollection
                    .Find(x => x.Id == review.TourId)
                    .FirstOrDefaultAsync();

                if (tour != null)
                    review.TourName = tour.Name;
            }

            return result;
        }

        public async Task<ReviewDetailDto> GetByIdAsync(string id)
        {
            var review = await _reviewCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (review == null)
                return null;

            var result = _mapper.Map<ReviewDetailDto>(review);

            var tour = await _tourCollection
                .Find(x => x.Id == review.TourId)
                .FirstOrDefaultAsync();

            if (tour != null)
                result.TourName = tour.Name;

            return result;
        }

        public async Task CreateAsync(CreateReviewDto createReviewDto)
        {
            var review = _mapper.Map<Review>(createReviewDto);

            review.IsRead = false;
            review.IsActive = true;

            await _reviewCollection.InsertOneAsync(review);

            await RecalculateTourRatingAsync(review.TourId);
        }

        public async Task UpdateAsync(UpdateReviewDto updateReviewDto)
        {
            var review = _mapper.Map<Review>(updateReviewDto);

            review.UpdatedAt = DateTime.UtcNow;

            await _reviewCollection.FindOneAndReplaceAsync(
                x => x.Id == review.Id,
                review);
        }

        public async Task DeleteAsync(string id)
        {
            var review = await _reviewCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            await _reviewCollection.DeleteOneAsync(
                x => x.Id == id);

            if (review != null)
                await RecalculateTourRatingAsync(review.TourId);
        }

        public async Task<List<ReviewListDto>> GetByTourAsync(string tourId)
        {
            var reviews = await _reviewCollection
                .Find(x => x.TourId == tourId && x.IsActive)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();

            return _mapper.Map<List<ReviewListDto>>(reviews);
        }

        public async Task<List<ReviewListDto>> GetByUserAsync(string userId)
        {
            var reviews = await _reviewCollection
                .Find(x => x.UserId == userId)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();

            var result = _mapper.Map<List<ReviewListDto>>(reviews);

            var tourIds = result.Select(x => x.TourId).Distinct().ToList();
            var tours = await _tourCollection
                .Find(x => tourIds.Contains(x.Id))
                .ToListAsync();

            foreach (var review in result)
                review.TourName = tours.FirstOrDefault(x => x.Id == review.TourId)?.Name ?? "-";

            return result;
        }

        public async Task<bool> HasUserReviewedAsync(string userId, string tourId)
        {
            return await _reviewCollection
                .Find(x => x.UserId == userId && x.TourId == tourId)
                .AnyAsync();
        }

        /// <summary>
        /// Turun ortalama puanını ve yorum sayısını Aggregation Pipeline
        /// ile hesaplayıp Tour dokümanına (denormalize) yazar.
        /// </summary>
        private async Task RecalculateTourRatingAsync(string tourId)
        {
            var stats = await _reviewCollection.Aggregate()
                .Match(x => x.TourId == tourId && x.IsActive)
                .Group(x => x.TourId, g => new
                {
                    Average = g.Average(x => x.Rating),
                    Count = g.Count()
                })
                .FirstOrDefaultAsync();

            var update = Builders<Tour>.Update
                .Set(x => x.AverageRating, stats == null ? 0 : Math.Round(stats.Average, 1))
                .Set(x => x.ReviewCount, stats?.Count ?? 0);

            await _tourCollection.UpdateOneAsync(x => x.Id == tourId, update);
        }

        public async Task MarkAsReadAsync(string id)
        {
            var update = Builders<Review>.Update
                .Set(x => x.IsRead, true)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);

            await _reviewCollection.UpdateOneAsync(
                x => x.Id == id,
                update);
        }
    }
}