using Travel.Web.DTOs.ContactCardDtos;

namespace Travel.Web.Services.ContactCardServices
{
    public interface IContactCardService
    {
        Task<List<ResultContactCardDto>> GetAllAsync();

        Task<ResultContactCardDto> GetByIdAsync(string id);

        Task CreateAsync(
            CreateContactCardDto contactCardCreateDto);

        Task UpdateAsync(
            UpdateContactCardDto contactCardUpdateDto);

        Task DeleteAsync(string id);
    }
}