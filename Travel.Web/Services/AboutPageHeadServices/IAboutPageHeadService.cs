using Travel.Web.DTOs.AboutPageHeadDtos;

namespace Travel.Web.Services.AboutPageHeadServices
{
    public interface IAboutPageHeadService
    {
        Task<List<AboutPageHeadResultDto>> GetAllAsync();

        Task<AboutPageHeadResultDto> GetByIdAsync(string id);

        Task CreateAsync(AboutPageHeadCreateDto aboutPageHeadCreateDto);

        Task UpdateAsync(AboutPageHeadUpdateDto aboutPageHeadUpdateDto);
    }
}