using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Default_Reviews
{
    public class _DefaultReviewsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
