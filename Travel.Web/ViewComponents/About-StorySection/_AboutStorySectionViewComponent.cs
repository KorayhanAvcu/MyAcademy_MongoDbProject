using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.About_StorySection
{
    public class _AboutStorySectionViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
