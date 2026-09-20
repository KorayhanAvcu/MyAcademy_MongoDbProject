using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.ContactFormDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.ContactFormServices
{
    public class ContactFormService : IContactFormService
    {
        private readonly IMongoCollection<ContactForm> _contactFormCollection;
        private readonly IMapper _mapper;

        public ContactFormService(
            IDatabaseSettings databaseSettings,
            IMapper mapper)
        {
            var client = new MongoClient(
                databaseSettings.ConnectionString);

            var database = client.GetDatabase(
                databaseSettings.DatabaseName);

            _contactFormCollection =
                database.GetCollection<ContactForm>(
                    databaseSettings.ContactFormCollectionName);

            _mapper = mapper;
        }

        public async Task<List<ResultContactFormDto>> GetAllAsync()
        {
            var contactForms =
                await _contactFormCollection
                    .Find(x => true)
                    .ToListAsync();

            return _mapper.Map<List<ResultContactFormDto>>(
                contactForms);
        }

        public async Task<ResultContactFormDto> GetByIdAsync(string id)
        {
            var contactForm =
                await _contactFormCollection
                    .Find(x => x.Id == id)
                    .FirstOrDefaultAsync();

            return _mapper.Map<ResultContactFormDto>(
                contactForm);
        }

        public async Task CreateAsync(
            CreateContactFormDto contactFormCreateDto)
        {
            var contactForm =
                _mapper.Map<ContactForm>(
                    contactFormCreateDto);

            await _contactFormCollection
                .InsertOneAsync(contactForm);
        }

        public async Task UpdateAsync(
            UpdateContactFormDto contactFormUpdateDto)
        {
            var contactForm =
                _mapper.Map<ContactForm>(
                    contactFormUpdateDto);

            await _contactFormCollection.ReplaceOneAsync(
                x => x.Id == contactFormUpdateDto.Id,
                contactForm);
        }

        public async Task DeleteAsync(string id)
        {
            await _contactFormCollection
                .DeleteOneAsync(x => x.Id == id);
        }
    }
}