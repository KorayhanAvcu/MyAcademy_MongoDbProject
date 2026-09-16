using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.About_Values
{
    public class _AboutValuesViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
