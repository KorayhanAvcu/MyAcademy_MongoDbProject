using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.AltBannerDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.AltBannerServices
{
    public class AltBannerService : IAltBannerService
    {
        private readonly IMongoCollection<AltBanner> _altBannerCollection;
        private readonly IMapper _mapper;

        public AltBannerService(
            IDatabaseSettings databaseSettings,
            IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _altBannerCollection = database.GetCollection<AltBanner>(
                databaseSettings.AltBannerCollectionName);

            _mapper = mapper;
        }

        public async Task<List<ResultAltBannerDto>> GetAllAsync()
        {
            var altBanners = await _altBannerCollection
                .Find(x => true)
                .ToListAsync();

            return _mapper.Map<List<ResultAltBannerDto>>(altBanners);
        }

        public async Task<ResultAltBannerDto> GetByIdAsync(string id)
        {
            var altBanner = await _altBannerCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<ResultAltBannerDto>(altBanner);
        }

        public async Task CreateAsync(CreateAltBannerDto createAltBannerDto)
        {
            var altBannerCount = await _altBannerCollection
                .CountDocumentsAsync(FilterDefinition<AltBanner>.Empty);

            if (altBannerCount >= 1)
            {
                throw new InvalidOperationException(
                    "Sadece bir adet Alt Banner oluşturulabilir.");
            }

            var altBanner = _mapper.Map<AltBanner>(createAltBannerDto);

            await _altBannerCollection.InsertOneAsync(altBanner);
        }

        public async Task UpdateAsync(UpdateAltBannerDto updateAltBannerDto)
        {
            var altBanner = _mapper.Map<AltBanner>(updateAltBannerDto);

            await _altBannerCollection.ReplaceOneAsync(
                x => x.Id == updateAltBannerDto.Id,
                altBanner);
        }
    }
}