using Travel.Web.DTOs.TourDtos;
using Travel.Web.Entities.Enums;
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

        Task SetStatusAsync(string id, TourStatus status);

        /// <summary>
        /// Kategori ve destinasyon adları doldurulmuş, aktif dile göre
        /// yerelleştirilmiş tur listesi.
        /// </summary>
        Task<List<TourListItemDto>> GetListItemsAsync(bool onlyActive, IEnumerable<string>? ids = null);

        /// <summary>En çok rezervasyon alan aktif turlar.</summary>
        Task<List<TourListItemDto>> GetPopularAsync(int count);

        Task<(string? CategoryName, string? Country, string? City)> GetLookupNamesAsync(string categoryId, string destinationId);
    }
}

