using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Tour_Comments
{
    public class _TourCommentsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();

        }
    }
}
