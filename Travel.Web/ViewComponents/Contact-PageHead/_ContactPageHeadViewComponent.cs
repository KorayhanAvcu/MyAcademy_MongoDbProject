using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.ContactPageHeadServices;

namespace Travel.Web.ViewComponents.Contact_PageHead
{
    public class _ContactPageHeadViewComponent(
        IContactPageHeadService contactPageHeadService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await contactPageHeadService.GetAllAsync();

            var contactPageHead = values.FirstOrDefault();

            return View(contactPageHead);
        }
    }
}