using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.AboutPageHeadDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.AboutPageHeadServices
{
    public class AboutPageHeadService : IAboutPageHeadService
    {
        private readonly IMongoCollection<AboutPageHead> _aboutPageHeadCollection;
        private readonly IMapper _mapper;

        public AboutPageHeadService(
            IDatabaseSettings databaseSettings,
            IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _aboutPageHeadCollection =database.GetCollection<AboutPageHead>(databaseSettings.AboutPageHeadCollectionName);

            _mapper = mapper;
        }

        public async Task<List<AboutPageHeadResultDto>> GetAllAsync()
        {
            var aboutPageHeads = await _aboutPageHeadCollection
                                                            .Find(x => true)
                                                            .ToListAsync();

            return _mapper.Map<List<AboutPageHeadResultDto>>(aboutPageHeads);
        }

        public async Task<AboutPageHeadResultDto> GetByIdAsync(string id)
        {
            var aboutPageHead = await _aboutPageHeadCollection
                                                            .Find(x => x.Id == id)
                                                            .FirstOrDefaultAsync();

            return _mapper.Map<AboutPageHeadResultDto>(aboutPageHead);
        }

        public async Task CreateAsync(AboutPageHeadCreateDto aboutPageHeadCreateDto)
        {
            var aboutPageHead = _mapper.Map<AboutPageHead>(aboutPageHeadCreateDto);

            await _aboutPageHeadCollection.InsertOneAsync(aboutPageHead);
        }

        public async Task UpdateAsync(AboutPageHeadUpdateDto aboutPageHeadUpdateDto)
        {
            var aboutPageHead = _mapper.Map<AboutPageHead>(aboutPageHeadUpdateDto);

            await _aboutPageHeadCollection.ReplaceOneAsync(x => x.Id == aboutPageHeadUpdateDto.Id,aboutPageHead);
        }
    }
}