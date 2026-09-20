using Travel.Web.DTOs.ContactPageHeadDtos;

namespace Travel.Web.Services.ContactPageHeadServices
{
    public interface IContactPageHeadService
    {
        Task<List<ResultContactPageHeadDto>> GetAllAsync();

        Task<ResultContactPageHeadDto> GetByIdAsync(string id);

        Task CreateAsync(CreateContactPageHeadDto dto);

        Task UpdateAsync(UpdateContactPageHeadDto dto);

        Task DeleteAsync(string id);
    }
}