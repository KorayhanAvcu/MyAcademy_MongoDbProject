
using Travel.Web.Entities.Tour;

namespace Travel.Web.Services.TourServices
{
    public interface ITourService
    {
        Task<List<Tour>> GetAllAsync();

        Task<Tour?> GetByIdAsync(string id);

        Task CreateAsync(Tour tour);

        Task UpdateAsync(Tour tour);

        Task DeleteAsync(string id);
    }
}

