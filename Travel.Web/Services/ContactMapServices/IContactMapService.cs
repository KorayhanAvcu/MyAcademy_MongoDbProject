using Travel.Web.DTOs.ContactMapDtos;

namespace Travel.Web.Services.ContactMapServices
{
    public interface IContactMapService
    {
        Task<List<ResultContactMapDto>> GetAllAsync();

        Task<ResultContactMapDto> GetByIdAsync(string id);

        Task CreateAsync(
            CreateContactMapDto contactMapCreateDto);

        Task UpdateAsync(
            UpdateContactMapDto contactMapUpdateDto);

        Task DeleteAsync(string id);
    }
}