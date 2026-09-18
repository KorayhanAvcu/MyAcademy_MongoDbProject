
using Travel.Web.DTOs.AboutStorySectionDtos;

namespace Travel.Web.Services.AboutStorySectionServices
{
    public interface IAboutStorySectionService
    {
        Task<List<AboutStorySectionResultDto>> GetAllAsync();

        Task<AboutStorySectionResultDto> GetByIdAsync(string id);

        Task CreateAsync(AboutStorySectionCreateDto dto);

        Task UpdateAsync(AboutStorySectionUpdateDto dto);

        Task DeleteAsync(string id);
    }
}

