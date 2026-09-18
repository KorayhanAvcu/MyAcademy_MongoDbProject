using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.AboutTeamMemberDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.About;
using Travel.Web.Settings;

namespace Travel.Web.Services.AboutTeamMemberServices
{
    public class AboutTeamMemberService : IAboutTeamMemberService
    {
        private readonly IMongoCollection<AboutTeamMember> _aboutTeamMemberCollection;
        private readonly IMapper _mapper;

        public AboutTeamMemberService(
            IDatabaseSettings databaseSettings,
            IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _aboutTeamMemberCollection = database.GetCollection<AboutTeamMember>(
                databaseSettings.AboutTeamMemberCollectionName);

            _mapper = mapper;
        }

        public async Task<List<AboutTeamMemberResultDto>> GetAllAsync()
        {
            var aboutTeamMembers = await _aboutTeamMemberCollection
                .Find(x => true)
                .ToListAsync();

            return _mapper.Map<List<AboutTeamMemberResultDto>>(aboutTeamMembers);
        }

        public async Task<AboutTeamMemberResultDto> GetByIdAsync(string id)
        {
            var aboutTeamMember = await _aboutTeamMemberCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<AboutTeamMemberResultDto>(aboutTeamMember);
        }

        public async Task<AboutTeamMemberUpdateDto> GetByIdForUpdateAsync(string id)
        {
            var aboutTeamMember = await _aboutTeamMemberCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<AboutTeamMemberUpdateDto>(aboutTeamMember);
        }

        public async Task CreateAsync(AboutTeamMemberCreateDto aboutTeamMemberCreateDto)
        {
            var aboutTeamMember = _mapper.Map<AboutTeamMember>(aboutTeamMemberCreateDto);

            await _aboutTeamMemberCollection.InsertOneAsync(aboutTeamMember);
        }

        public async Task UpdateAsync(AboutTeamMemberUpdateDto aboutTeamMemberUpdateDto)
        {
            var aboutTeamMember = _mapper.Map<AboutTeamMember>(aboutTeamMemberUpdateDto);

            aboutTeamMember.UpdatedAt = DateTime.UtcNow;

            await _aboutTeamMemberCollection.ReplaceOneAsync(
                x => x.Id == aboutTeamMemberUpdateDto.Id,
                aboutTeamMember);
        }
    }
}