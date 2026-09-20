using Travel.Web.DTOs.ContactFAQDtos;

namespace Travel.Web.Services.ContactFAQServices
{
    public interface IContactFAQService
    {
        Task<List<ResultContactFAQDto>> GetAllAsync();

        Task<ResultContactFAQDto> GetByIdAsync(string id);

        Task CreateAsync(
            CreateContactFAQDto contactFAQCreateDto);

        Task UpdateAsync(
            UpdateContactFAQDto contactFAQUpdateDto);

        Task DeleteAsync(string id);
    }
}