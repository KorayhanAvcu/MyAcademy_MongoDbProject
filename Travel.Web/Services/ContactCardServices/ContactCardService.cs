using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.ContactCardDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.ContactCardServices
{
    public class ContactCardService : IContactCardService
    {
        private readonly IMongoCollection<ContactCard> _contactCardCollection;
        private readonly IMapper _mapper;

        public ContactCardService(
            IDatabaseSettings databaseSettings,
            IMapper mapper)
        {
            var client = new MongoClient(
                databaseSettings.ConnectionString);

            var database = client.GetDatabase(
                databaseSettings.DatabaseName);

            _contactCardCollection =
                database.GetCollection<ContactCard>(
                    databaseSettings.ContactCardCollectionName);

            _mapper = mapper;
        }

        public async Task<List<ResultContactCardDto>> GetAllAsync()
        {
            var contactCards = await _contactCardCollection
                .Find(x => true)
                .ToListAsync();

            return _mapper.Map<List<ResultContactCardDto>>(
                contactCards);
        }

        public async Task<ResultContactCardDto> GetByIdAsync(string id)
        {
            var contactCard = await _contactCardCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<ResultContactCardDto>(
                contactCard);
        }

        public async Task CreateAsync(
            CreateContactCardDto contactCardCreateDto)
        {
            var contactCard =
                _mapper.Map<ContactCard>(
                    contactCardCreateDto);

            await _contactCardCollection
                .InsertOneAsync(contactCard);
        }

        public async Task UpdateAsync(
            UpdateContactCardDto contactCardUpdateDto)
        {
            var contactCard =
                _mapper.Map<ContactCard>(
                    contactCardUpdateDto);

            await _contactCardCollection.ReplaceOneAsync(
                x => x.Id == contactCardUpdateDto.Id,
                contactCard);
        }

        public async Task DeleteAsync(string id)
        {
            await _contactCardCollection
                .DeleteOneAsync(x => x.Id == id);
        }
    }
}