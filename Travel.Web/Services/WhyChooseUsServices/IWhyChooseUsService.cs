using Travel.Web.DTOs.WhyChooseUsDtos;

namespace Travel.Web.Services.WhyChooseUsServices
{
    public interface IWhyChooseUsService
    {
        Task CreateAsync(CreateWhyChooseUsDto createWhyChooseUsDto);

        Task DeleteAsync(string id);

        Task<List<ResultWhyChooseUsDto>> GetAllAsync();

        Task<ResultWhyChooseUsDto> GetByIdAsync(string id);

        Task UpdateAsync(UpdateWhyChooseUsDto updateWhyChooseUsDto);
    }
}