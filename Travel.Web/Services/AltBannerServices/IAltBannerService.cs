using Travel.Web.DTOs.AltBannerDtos;

namespace Travel.Web.Services.AltBannerServices
{
    public interface IAltBannerService
    {
        Task<List<ResultAltBannerDto>> GetAllAsync();

        Task<ResultAltBannerDto> GetByIdAsync(string id);

        Task CreateAsync(CreateAltBannerDto createAltBannerDto);

        Task UpdateAsync(UpdateAltBannerDto updateAltBannerDto);
    }
}