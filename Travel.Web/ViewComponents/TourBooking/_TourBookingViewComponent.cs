using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.TourBooking
{
    public class _TourBookingViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();

        }
    }
}
