using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Tour_About
{
    public class _TourAboutViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
