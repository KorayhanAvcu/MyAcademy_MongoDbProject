using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Contact_Form
{
    public class _ContactFormViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
