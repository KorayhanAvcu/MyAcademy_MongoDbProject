using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Contact_FAQ
{
    public class _ContactFAQViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
