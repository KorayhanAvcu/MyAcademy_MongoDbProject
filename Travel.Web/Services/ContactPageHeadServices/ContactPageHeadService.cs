using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.ContactPageHeadDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.ContactPageHeadServices
{
    public class ContactPageHeadService : IContactPageHeadService
    {
        private readonly IMongoCollection<ContactPageHead> _contactPageHeadCollection;
        private readonly IMapper _mapper;

        public ContactPageHeadService(
            IDatabaseSettings databaseSettings,
            IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _contactPageHeadCollection =
                database.GetCollection<ContactPageHead>(
                    databaseSettings.ContactPageHeadCollectionName);

            _mapper = mapper;
        }

        public async Task<List<ResultContactPageHeadDto>> GetAllAsync()
        {
            var contactPageHeads = await _contactPageHeadCollection
                .Find(x => true)
                .ToListAsync();

            return _mapper.Map<List<ResultContactPageHeadDto>>(
                contactPageHeads);
        }

        public async Task<ResultContactPageHeadDto> GetByIdAsync(string id)
        {
            var contactPageHead = await _contactPageHeadCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<ResultContactPageHeadDto>(
                contactPageHead);
        }

        public async Task CreateAsync(
            CreateContactPageHeadDto contactPageHeadCreateDto)
        {
            var contactPageHead =
                _mapper.Map<ContactPageHead>(
                    contactPageHeadCreateDto);

            await _contactPageHeadCollection
                .InsertOneAsync(contactPageHead);
        }

        public async Task UpdateAsync(
            UpdateContactPageHeadDto contactPageHeadUpdateDto)
        {
            var contactPageHead =
                _mapper.Map<ContactPageHead>(
                    contactPageHeadUpdateDto);

            await _contactPageHeadCollection.ReplaceOneAsync(
                x => x.Id == contactPageHeadUpdateDto.Id,
                contactPageHead);
        }

        public async Task DeleteAsync(string id)
        {
            await _contactPageHeadCollection
                .DeleteOneAsync(x => x.Id == id);
        }
    }
}