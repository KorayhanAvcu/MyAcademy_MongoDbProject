using Travel.Web.DTOs.AboutStatDtos;

namespace Travel.Web.Services.AboutStatServices
{
    public interface IAboutStatService
    {
        Task<List<AboutStatResultDto>> GetAllAsync();
        Task<AboutStatResultDto> GetByIdAsync(string id);
        Task<AboutStatUpdateDto> GetByIdForUpdateAsync(string id);
        Task CreateAsync(AboutStatCreateDto dto);
        Task UpdateAsync(AboutStatUpdateDto dto);
    }
}