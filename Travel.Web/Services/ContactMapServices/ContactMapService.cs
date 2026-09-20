using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.ContactMapDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.ContactMapServices
{
    public class ContactMapService : IContactMapService
    {
        private readonly IMongoCollection<ContactMap> _contactMapCollection;
        private readonly IMapper _mapper;

        public ContactMapService(
            IDatabaseSettings databaseSettings,
            IMapper mapper)
        {
            var client = new MongoClient(
                databaseSettings.ConnectionString);

            var database = client.GetDatabase(
                databaseSettings.DatabaseName);

            _contactMapCollection =
                database.GetCollection<ContactMap>(
                    databaseSettings.ContactMapCollectionName);

            _mapper = mapper;
        }

        public async Task<List<ResultContactMapDto>> GetAllAsync()
        {
            var contactMaps =
                await _contactMapCollection
                    .Find(x => true)
                    .ToListAsync();

            return _mapper.Map<List<ResultContactMapDto>>(
                contactMaps);
        }

        public async Task<ResultContactMapDto> GetByIdAsync(string id)
        {
            var contactMap =
                await _contactMapCollection
                    .Find(x => x.Id == id)
                    .FirstOrDefaultAsync();

            return _mapper.Map<ResultContactMapDto>(
                contactMap);
        }

        public async Task CreateAsync(
            CreateContactMapDto contactMapCreateDto)
        {
            var contactMap =
                _mapper.Map<ContactMap>(
                    contactMapCreateDto);

            await _contactMapCollection
                .InsertOneAsync(contactMap);
        }

        public async Task UpdateAsync(
            UpdateContactMapDto contactMapUpdateDto)
        {
            var contactMap =
                _mapper.Map<ContactMap>(
                    contactMapUpdateDto);

            await _contactMapCollection.ReplaceOneAsync(
                x => x.Id == contactMapUpdateDto.Id,
                contactMap);
        }

        public async Task DeleteAsync(string id)
        {
            await _contactMapCollection
                .DeleteOneAsync(x => x.Id == id);
        }
    }
}