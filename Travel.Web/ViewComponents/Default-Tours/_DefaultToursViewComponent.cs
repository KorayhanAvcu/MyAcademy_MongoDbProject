using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Default_Tours
{
    public class _DefaultToursViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
