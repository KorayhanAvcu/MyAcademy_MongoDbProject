using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.AboutValueDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.About;
using Travel.Web.Settings;

namespace Travel.Web.Services.AboutValueServices
{
    public class AboutValueService : IAboutValueService
    {
        private readonly IMongoCollection<AboutValue> _aboutValueCollection;
        private readonly IMapper _mapper;

        public AboutValueService(
            IDatabaseSettings databaseSettings,
            IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _aboutValueCollection = database.GetCollection<AboutValue>(
                databaseSettings.AboutValueCollectionName);

            _mapper = mapper;
        }

        public async Task<List<AboutValueResultDto>> GetAllAsync()
        {
            var aboutValues = await _aboutValueCollection
                .Find(x => true)
                .ToListAsync();

            return _mapper.Map<List<AboutValueResultDto>>(aboutValues);
        }

        public async Task<AboutValueResultDto> GetByIdAsync(string id)
        {
            var aboutValue = await _aboutValueCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<AboutValueResultDto>(aboutValue);
        }

        public async Task<AboutValueUpdateDto> GetByIdForUpdateAsync(string id)
        {
            var aboutValue = await _aboutValueCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<AboutValueUpdateDto>(aboutValue);
        }

        public async Task CreateAsync(AboutValueCreateDto aboutValueCreateDto)
        {
            var aboutValue = _mapper.Map<AboutValue>(aboutValueCreateDto);

            await _aboutValueCollection.InsertOneAsync(aboutValue);
        }

        public async Task UpdateAsync(AboutValueUpdateDto aboutValueUpdateDto)
        {
            var aboutValue = _mapper.Map<AboutValue>(aboutValueUpdateDto);

            aboutValue.UpdatedAt = DateTime.UtcNow;

            await _aboutValueCollection.ReplaceOneAsync(
                x => x.Id == aboutValueUpdateDto.Id,
                aboutValue);
        }
    }
}