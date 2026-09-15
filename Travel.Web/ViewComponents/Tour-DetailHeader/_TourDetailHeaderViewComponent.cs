using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Tour_DetailHeader
{
    public class _TourDetailHeaderViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
