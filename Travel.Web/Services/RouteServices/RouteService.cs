using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Travel.Web.DTOs.RouteDtos;
using Travel.Web.Settings;
using route = Travel.Web.Entities.Route;

namespace Travel.Web.Services.RouteServices
{
    public class RouteService : IRouteService
    {
        private readonly IMongoCollection<route> _routeCollection;
        private readonly IMapper _mapper;

        public RouteService(
            IMapper mapper,
            IDatabaseSettings databaseSettings)
        {
            _mapper = mapper;

            var client = new MongoClient(
                databaseSettings.ConnectionString);

            var database = client.GetDatabase(
                databaseSettings.DatabaseName);

            _routeCollection = database.GetCollection<route>(
                databaseSettings.RouteCollectionName);
        }

        public async Task CreateAsync(CreateRouteDto createRouteDto)
        {
            var route = _mapper.Map<route>(createRouteDto);

            await _routeCollection.InsertOneAsync(route);
        }

        public async Task DeleteAsync(string id)
        {
            await _routeCollection.DeleteOneAsync(
                x => x.Id == id);
        }

        public async Task<List<ResultRouteDto>> GetAllAsync()
        {
            var routes = await _routeCollection
                .AsQueryable()
                .ToListAsync();

            return _mapper.Map<List<ResultRouteDto>>(routes);
        }

        public async Task<List<ResultRouteDto>> GetAllByDestinationAsync(
            string destinationId)
        {
            var routes = await _routeCollection
                .Find(x => x.DestinationId == destinationId)
                .ToListAsync();

            return _mapper.Map<List<ResultRouteDto>>(routes);
        }

        public async Task<ResultRouteDto> GetByIdAsync(string id)
        {
            var route = await _routeCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<ResultRouteDto>(route);
        }

        public async Task UpdateAsync(
            UpdateRouteDto updateRouteDto)
        {
            var route = _mapper.Map<route>(updateRouteDto);

            await _routeCollection.FindOneAndReplaceAsync(
                x => x.Id == route.Id,
                route);
        }
    }
}