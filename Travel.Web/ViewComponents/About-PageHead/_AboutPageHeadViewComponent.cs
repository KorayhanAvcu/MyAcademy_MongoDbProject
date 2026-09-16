using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.About_PageHead
{
    public class _AboutPageHeadViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
