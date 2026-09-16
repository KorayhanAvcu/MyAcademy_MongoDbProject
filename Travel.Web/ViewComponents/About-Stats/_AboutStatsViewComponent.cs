using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.About_Stats
{
    public class _AboutStatsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
