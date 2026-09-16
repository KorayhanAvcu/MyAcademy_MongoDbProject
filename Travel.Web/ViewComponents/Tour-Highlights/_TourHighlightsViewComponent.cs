using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Tour_Highlights
{
    public class _TourHighlightsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();

        }
    }
}
