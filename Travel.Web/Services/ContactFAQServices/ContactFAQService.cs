using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.ContactFAQDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.ContactFAQServices
{
    public class ContactFAQService : IContactFAQService
    {
        private readonly IMongoCollection<ContactFAQ> _contactFAQCollection;
        private readonly IMapper _mapper;

        public ContactFAQService(
            IDatabaseSettings databaseSettings,
            IMapper mapper)
        {
            var client = new MongoClient(
                databaseSettings.ConnectionString);

            var database = client.GetDatabase(
                databaseSettings.DatabaseName);

            _contactFAQCollection =
                database.GetCollection<ContactFAQ>(
                    databaseSettings.ContactFAQCollectionName);

            _mapper = mapper;
        }

        public async Task<List<ResultContactFAQDto>> GetAllAsync()
        {
            var contactFAQs =
                await _contactFAQCollection
                    .Find(x => true)
                    .ToListAsync();

            return _mapper.Map<List<ResultContactFAQDto>>(
                contactFAQs);
        }

        public async Task<ResultContactFAQDto> GetByIdAsync(string id)
        {
            var contactFAQ =
                await _contactFAQCollection
                    .Find(x => x.Id == id)
                    .FirstOrDefaultAsync();

            return _mapper.Map<ResultContactFAQDto>(
                contactFAQ);
        }

        public async Task CreateAsync(
            CreateContactFAQDto contactFAQCreateDto)
        {
            var contactFAQ =
                _mapper.Map<ContactFAQ>(
                    contactFAQCreateDto);

            await _contactFAQCollection
                .InsertOneAsync(contactFAQ);
        }

        public async Task UpdateAsync(
            UpdateContactFAQDto contactFAQUpdateDto)
        {
            var contactFAQ =
                _mapper.Map<ContactFAQ>(
                    contactFAQUpdateDto);

            await _contactFAQCollection.ReplaceOneAsync(
                x => x.Id == contactFAQUpdateDto.Id,
                contactFAQ);
        }

        public async Task DeleteAsync(string id)
        {
            await _contactFAQCollection
                .DeleteOneAsync(x => x.Id == id);
        }
    }
}