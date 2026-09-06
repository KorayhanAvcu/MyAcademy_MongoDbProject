using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.WhyChooseUsDtos;
using Travel.Web.Settings;
using whyChooseUs = Travel.Web.Entities.WhyChooseUs;

namespace Travel.Web.Services.WhyChooseUsServices
{
    public class WhyChooseUsService : IWhyChooseUsService
    {
        private readonly IMongoCollection<whyChooseUs> _whyChooseUsCollection;
        private readonly IMapper _mapper;

        public WhyChooseUsService(
            IMapper mapper,
            IDatabaseSettings databaseSettings)
        {
            _mapper = mapper;

            var client = new MongoClient(
                databaseSettings.ConnectionString);

            var database = client.GetDatabase(
                databaseSettings.DatabaseName);

            _whyChooseUsCollection = database.GetCollection<whyChooseUs>(
                databaseSettings.WhyChooseUsCollectionName);
        }

        public async Task CreateAsync(
            CreateWhyChooseUsDto createWhyChooseUsDto)
        {
            var whyChooseUs = _mapper.Map<whyChooseUs>(
                createWhyChooseUsDto);

            await _whyChooseUsCollection.InsertOneAsync(
                whyChooseUs);
        }

        public async Task DeleteAsync(string id)
        {
            await _whyChooseUsCollection.DeleteOneAsync(
                x => x.Id == id);
        }

        public async Task<List<ResultWhyChooseUsDto>> GetAllAsync()
        {
            var whyChooseUs = await _whyChooseUsCollection
                .AsQueryable()
                .ToListAsync();

            return _mapper.Map<List<ResultWhyChooseUsDto>>(
                whyChooseUs);
        }

        public async Task<ResultWhyChooseUsDto> GetByIdAsync(
            string id)
        {
            var whyChooseUs = await _whyChooseUsCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<ResultWhyChooseUsDto>(
                whyChooseUs);
        }

        public async Task UpdateAsync(
            UpdateWhyChooseUsDto updateWhyChooseUsDto)
        {
            var whyChooseUs = _mapper.Map<whyChooseUs>(
                updateWhyChooseUsDto);

            await _whyChooseUsCollection.FindOneAndReplaceAsync(
                x => x.Id == whyChooseUs.Id,
                whyChooseUs);
        }
    }
}