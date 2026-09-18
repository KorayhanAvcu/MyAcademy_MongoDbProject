using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.AboutTimelineServices;

namespace Travel.Web.ViewComponents.About_Timeline
{
    public class _AboutTimelineViewComponent(
        IAboutTimelineService aboutTimelineService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await aboutTimelineService.GetAllAsync();

            return View(values);
        }
    }
}