using Microsoft.AspNetCore.Mvc;
using Travel.Web.Entities.Enums;
using Travel.Web.Services.DestinationServices;
using Travel.Web.Services.TourServices;

namespace Travel.Web.ViewComponents.Default_Destination
{
    public class _DefaultDestinationViewComponent(
        IDestinationService _destinationService,
        ITourService _tourService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var destinations = (await _destinationService.GetAllAsync())
                .Where(x => x.IsActive)
                .ToList();

            var tours = await _tourService.GetAllAsync();

            // Popülerlik: destinasyondaki aktif turların toplam rezervasyon sayısı
            var popularity = tours
                .Where(x => x.Status == TourStatus.Active)
                .GroupBy(x => x.DestinationId)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.ReservationCount) * 10 + g.Count());

            var values = destinations
                .OrderByDescending(x => popularity.TryGetValue(x.Id, out var score) ? score : 0)
                .Take(5)
                .ToList();

            return View(values);
        }
    }
}
