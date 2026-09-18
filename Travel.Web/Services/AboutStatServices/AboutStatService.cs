using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.AboutStatDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.About;
using Travel.Web.Settings;

namespace Travel.Web.Services.AboutStatServices
{
    public class AboutStatService : IAboutStatService
    {
        private readonly IMongoCollection<AboutStat> _aboutStatCollection;
        private readonly IMapper _mapper;

        public AboutStatService(
            IDatabaseSettings databaseSettings,
            IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _aboutStatCollection = database.GetCollection<AboutStat>(
                databaseSettings.AboutStatCollectionName);

            _mapper = mapper;
        }

        public async Task<List<AboutStatResultDto>> GetAllAsync()
        {
            var aboutStats = await _aboutStatCollection
                .Find(x => true)
                .ToListAsync();

            return _mapper.Map<List<AboutStatResultDto>>(aboutStats);
        }

        public async Task<AboutStatResultDto> GetByIdAsync(string id)
        {
            var aboutStat = await _aboutStatCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<AboutStatResultDto>(aboutStat);
        }

        public async Task<AboutStatUpdateDto> GetByIdForUpdateAsync(string id)
        {
            var aboutStat = await _aboutStatCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<AboutStatUpdateDto>(aboutStat);
        }

        public async Task CreateAsync(AboutStatCreateDto aboutStatCreateDto)
        {
            var aboutStat = _mapper.Map<AboutStat>(aboutStatCreateDto);

            await _aboutStatCollection.InsertOneAsync(aboutStat);
        }

        public async Task UpdateAsync(AboutStatUpdateDto aboutStatUpdateDto)
        {
            var aboutStat = _mapper.Map<AboutStat>(aboutStatUpdateDto);

            aboutStat.UpdatedAt = DateTime.UtcNow;

            await _aboutStatCollection.ReplaceOneAsync(
                x => x.Id == aboutStatUpdateDto.Id,
                aboutStat);
        }
    }
}