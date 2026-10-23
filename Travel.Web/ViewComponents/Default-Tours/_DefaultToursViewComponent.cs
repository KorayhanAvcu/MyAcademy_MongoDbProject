using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.TourServices;

namespace Travel.Web.ViewComponents.Default_Tours
{
    public class _DefaultToursViewComponent(ITourService _tourService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // En çok rezervasyon alan 6 aktif tur
            var values = await _tourService.GetPopularAsync(6);

            return View(values);
        }
    }
}
