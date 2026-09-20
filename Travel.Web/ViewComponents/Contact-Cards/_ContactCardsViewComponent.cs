using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.ContactCardServices;

namespace Travel.Web.ViewComponents.Contact_Card
{
    public class _ContactCardsViewComponent(
        IContactCardService contactCardService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await contactCardService.GetAllAsync();

            return View(values);
        }
    }
}