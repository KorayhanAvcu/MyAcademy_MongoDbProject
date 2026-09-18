using Travel.Web.DTOs.AboutTeamMemberDtos;

namespace Travel.Web.Services.AboutTeamMemberServices
{
    public interface IAboutTeamMemberService
    {
        Task<List<AboutTeamMemberResultDto>> GetAllAsync();
        Task<AboutTeamMemberResultDto> GetByIdAsync(string id);
        Task<AboutTeamMemberUpdateDto> GetByIdForUpdateAsync(string id);
        Task CreateAsync(AboutTeamMemberCreateDto dto);
        Task UpdateAsync(AboutTeamMemberUpdateDto dto);
    }
}