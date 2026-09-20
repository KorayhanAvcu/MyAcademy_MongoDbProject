using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.ContactMessageDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.ContactMessageServices
{
    public class ContactMessageService : IContactMessageService
    {
        private readonly IMongoCollection<ContactMessage> _contactMessageCollection;
        private readonly IMapper _mapper;

        public ContactMessageService(
            IDatabaseSettings databaseSettings,
            IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _contactMessageCollection =
                database.GetCollection<ContactMessage>(
                    databaseSettings.ContactMessageCollectionName);

            _mapper = mapper;
        }

        public async Task<List<ResultContactMessageDto>> GetAllAsync()
        {
            var contactMessages = await _contactMessageCollection
                .Find(x => true)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();

            return _mapper.Map<List<ResultContactMessageDto>>(contactMessages);
        }

        public async Task<ResultContactMessageDto> GetByIdAsync(string id)
        {
            var contactMessage = await _contactMessageCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<ResultContactMessageDto>(contactMessage);
        }

        public async Task CreateAsync(
            CreateContactMessageDto contactMessageCreateDto)
        {
            var contactMessage =
                _mapper.Map<ContactMessage>(contactMessageCreateDto);

            contactMessage.IsRead = false;
            contactMessage.CreatedAt = DateTime.UtcNow;
            contactMessage.UpdatedAt = DateTime.UtcNow;

            await _contactMessageCollection.InsertOneAsync(contactMessage);
        }

        public async Task MarkAsReadAsync(string id)
        {
            var update = Builders<ContactMessage>.Update
                .Set(x => x.IsRead, true)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);

            await _contactMessageCollection.UpdateOneAsync(
                x => x.Id == id,
                update);
        }
    }
}