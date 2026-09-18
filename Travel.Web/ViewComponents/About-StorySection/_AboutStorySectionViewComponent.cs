using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.AboutStorySectionServices;

namespace Travel.Web.ViewComponents.About_StorySection
{
    public class _AboutStorySectionViewComponent(
        IAboutStorySectionService aboutStorySectionService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await aboutStorySectionService.GetAllAsync();

            var value = values.FirstOrDefault();

            return View(value);
        }
    }
}