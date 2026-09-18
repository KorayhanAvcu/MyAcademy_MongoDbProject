using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.AboutStatServices;

namespace Travel.Web.ViewComponents.About_Stats
{
    public class _AboutStatsViewComponent(
        IAboutStatService aboutStatService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await aboutStatService.GetAllAsync();

            return View(values);
        }
    }
}