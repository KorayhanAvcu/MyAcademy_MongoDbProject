using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Tour_Detail
{
    public class _TourDetailViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}