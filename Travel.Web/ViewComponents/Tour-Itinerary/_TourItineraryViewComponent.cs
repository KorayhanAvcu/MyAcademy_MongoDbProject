using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Tour_Itinerary
{
    public class _TourItineraryViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();

        }
    }
}
