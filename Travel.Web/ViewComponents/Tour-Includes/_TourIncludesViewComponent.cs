using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Tour_Includes
{
    public class _TourIncludesViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();

        }
    }
}
