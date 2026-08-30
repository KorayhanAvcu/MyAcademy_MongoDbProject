using Travel.Web.DTOs.BannerDtos;
using Travel.Web.DTOs.RouteDtos;

namespace Travel.Web.Services.RouteServices
{
    public interface IRouteService
    {
        Task<List<ResultRouteDto>> GetAllByCityAsync(string city);
        Task<List<ResultRouteDto>> GetAllAsync();
        Task<ResultRouteDto> GetByIdAsync(string id);
        Task CreateAsync(CreateRouteDto createRouteDto);
        Task DeleteAsync(string id);
        Task UpdateAsync(UpdateRouteDto updateRouteDto);

    }
}
