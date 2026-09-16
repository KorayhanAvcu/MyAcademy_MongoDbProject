using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.About_Timeline
{
    public class _AboutTimelineViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
