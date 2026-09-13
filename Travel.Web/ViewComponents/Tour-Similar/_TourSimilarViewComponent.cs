using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Tour_Similar
{
    public class _TourSimilarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();

        }
    }
}
