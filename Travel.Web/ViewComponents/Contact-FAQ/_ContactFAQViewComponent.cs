using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.ContactFAQServices;

namespace Travel.Web.ViewComponents.Contact_FAQ
{
    public class _ContactFAQViewComponent(
        IContactFAQService contactFAQService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await contactFAQService.GetAllAsync();

            return View(values);
        }
    }
}