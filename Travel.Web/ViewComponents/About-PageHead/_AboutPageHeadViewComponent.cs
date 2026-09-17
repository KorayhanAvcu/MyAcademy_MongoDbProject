using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.AboutPageHeadServices;

namespace Travel.Web.ViewComponents.About_PageHead
{
    public class _AboutPageHeadViewComponent(
        IAboutPageHeadService aboutPageHeadService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await aboutPageHeadService.GetAllAsync();

            var aboutPageHead = values.FirstOrDefault();

            return View(aboutPageHead);
        }
    }
}