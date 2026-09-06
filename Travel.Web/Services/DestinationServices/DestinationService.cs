using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Travel.Web.DTOs.DestinationDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.DestinationServices
{
    public class DestinationService : IDestinationService
    {
        private readonly IMongoCollection<Destination> _destinationCollection;
        private readonly IMapper _mapper;

        public DestinationService(
            IDatabaseSettings databaseSettings,
            IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _destinationCollection = database.GetCollection<Destination>(
                databaseSettings.DestinationCollectionName);

            _mapper = mapper;
        }

        public async Task CreateAsync(CreateDestinationDto createDestinationDto)
        {
            var destination = _mapper.Map<Destination>(createDestinationDto);

            await _destinationCollection.InsertOneAsync(destination);
        }

        public async Task DeleteAsync(string id)
        {
            await _destinationCollection.DeleteOneAsync(
                x => x.Id == id);
        }

        public async Task<List<ResultDestinationDto>> GetAllAsync()
        {
            var destinations = await _destinationCollection
                .AsQueryable()
                .ToListAsync();

            return _mapper.Map<List<ResultDestinationDto>>(destinations);
        }

        public async Task<ResultDestinationDto> GetByIdAsync(string id)
        {
            var destination = await _destinationCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<ResultDestinationDto>(destination);
        }

        public async Task UpdateAsync(UpdateDestinationDto updateDestinationDto)
        {
            var destination = _mapper.Map<Destination>(updateDestinationDto);

            await _destinationCollection.FindOneAndReplaceAsync(
                x => x.Id == destination.Id,
                destination);
        }
    }
}