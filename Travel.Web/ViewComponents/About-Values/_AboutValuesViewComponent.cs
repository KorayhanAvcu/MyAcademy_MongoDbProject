using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.AboutValueServices;

namespace Travel.Web.ViewComponents.About_Values
{
    public class _AboutValuesViewComponent(
        IAboutValueService aboutValueService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await aboutValueService.GetAllAsync();

            return View(values);
        }
    }
}