
using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.AboutStorySectionDtos;
using Travel.Web.Entities.About;
using Travel.Web.Settings;

namespace Travel.Web.Services.AboutStorySectionServices
{
    public class AboutStorySectionService : IAboutStorySectionService
    {
        private readonly IMongoCollection<AboutStorySection> _collection;
        private readonly IMapper _mapper;

        public AboutStorySectionService(
            IDatabaseSettings databaseSettings,
            IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _collection = database.GetCollection<AboutStorySection>(
                databaseSettings.AboutStorySectionCollectionName);

            _mapper = mapper;
        }

        public async Task<List<AboutStorySectionResultDto>> GetAllAsync()
        {
            var entities = await _collection
                .Find(x => true)
                .ToListAsync();

            return _mapper.Map<List<AboutStorySectionResultDto>>(entities);
        }

        public async Task<AboutStorySectionResultDto> GetByIdAsync(string id)
        {
            var entity = await _collection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<AboutStorySectionResultDto>(entity);
        }

        public async Task CreateAsync(AboutStorySectionCreateDto dto)
        {
            var entity = _mapper.Map<AboutStorySection>(dto);

            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(AboutStorySectionUpdateDto dto)
        {
            var entity = _mapper.Map<AboutStorySection>(dto);

            await _collection.ReplaceOneAsync(
                x => x.Id == dto.Id,
                entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(
                x => x.Id == id);
        }
    }
}
