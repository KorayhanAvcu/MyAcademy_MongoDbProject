using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.AboutTimelineDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.About;
using Travel.Web.Settings;

namespace Travel.Web.Services.AboutTimelineServices
{
    public class AboutTimelineService : IAboutTimelineService
    {
        private readonly IMongoCollection<AboutTimeline> _aboutTimelineCollection;
        private readonly IMapper _mapper;

        public AboutTimelineService(
            IDatabaseSettings databaseSettings,
            IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _aboutTimelineCollection = database.GetCollection<AboutTimeline>(
                databaseSettings.AboutTimelineCollectionName);

            _mapper = mapper;
        }

        public async Task<List<AboutTimelineResultDto>> GetAllAsync()
        {
            var aboutTimelines = await _aboutTimelineCollection
                .Find(x => true)
                .ToListAsync();

            return _mapper.Map<List<AboutTimelineResultDto>>(aboutTimelines);
        }

        public async Task<AboutTimelineResultDto> GetByIdAsync(string id)
        {
            var aboutTimeline = await _aboutTimelineCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<AboutTimelineResultDto>(aboutTimeline);
        }

        public async Task<AboutTimelineUpdateDto> GetByIdForUpdateAsync(string id)
        {
            var aboutTimeline = await _aboutTimelineCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<AboutTimelineUpdateDto>(aboutTimeline);
        }

        public async Task CreateAsync(AboutTimelineCreateDto aboutTimelineCreateDto)
        {
            var aboutTimeline = _mapper.Map<AboutTimeline>(aboutTimelineCreateDto);

            await _aboutTimelineCollection.InsertOneAsync(aboutTimeline);
        }

        public async Task UpdateAsync(AboutTimelineUpdateDto aboutTimelineUpdateDto)
        {
            var aboutTimeline = _mapper.Map<AboutTimeline>(aboutTimelineUpdateDto);

            aboutTimeline.UpdatedAt = DateTime.UtcNow;

            await _aboutTimelineCollection.ReplaceOneAsync(
                x => x.Id == aboutTimelineUpdateDto.Id,
                aboutTimeline);
        }
    }
}