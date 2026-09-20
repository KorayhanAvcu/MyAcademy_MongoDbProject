using Travel.Web.DTOs.ContactMessageDtos;

namespace Travel.Web.Services.ContactMessageServices
{
    public interface IContactMessageService
    {
        Task<List<ResultContactMessageDto>> GetAllAsync();
        Task<ResultContactMessageDto> GetByIdAsync(string id);
        Task CreateAsync(CreateContactMessageDto contactMessageCreateDto);
        Task MarkAsReadAsync(string id);
    }
}