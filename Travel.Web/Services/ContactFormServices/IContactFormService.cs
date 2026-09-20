using Travel.Web.DTOs.ContactFormDtos;

namespace Travel.Web.Services.ContactFormServices
{
    public interface IContactFormService
    {
        Task<List<ResultContactFormDto>> GetAllAsync();

        Task<ResultContactFormDto> GetByIdAsync(string id);

        Task CreateAsync(
            CreateContactFormDto contactFormCreateDto);

        Task UpdateAsync(
            UpdateContactFormDto contactFormUpdateDto);

        Task DeleteAsync(string id);
    }
}