using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Tour_Info
{
    public class _TourInfoViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();

        }
    }
}
