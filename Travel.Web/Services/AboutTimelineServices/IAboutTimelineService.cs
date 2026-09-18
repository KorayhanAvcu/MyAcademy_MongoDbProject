using Travel.Web.DTOs.AboutTimelineDtos;

namespace Travel.Web.Services.AboutTimelineServices
{
    public interface IAboutTimelineService
    {
        Task<List<AboutTimelineResultDto>> GetAllAsync();
        Task<AboutTimelineResultDto> GetByIdAsync(string id);
        Task<AboutTimelineUpdateDto> GetByIdForUpdateAsync(string id);
        Task CreateAsync(AboutTimelineCreateDto dto);
        Task UpdateAsync(AboutTimelineUpdateDto dto);
    }
}