using Travel.Web.DTOs.AboutValueDtos;

namespace Travel.Web.Services.AboutValueServices
{
    public interface IAboutValueService
    {
        Task<List<AboutValueResultDto>> GetAllAsync();
        Task<AboutValueResultDto> GetByIdAsync(string id);
        Task<AboutValueUpdateDto> GetByIdForUpdateAsync(string id);
        Task CreateAsync(AboutValueCreateDto dto);
        Task UpdateAsync(AboutValueUpdateDto dto);
    }
}