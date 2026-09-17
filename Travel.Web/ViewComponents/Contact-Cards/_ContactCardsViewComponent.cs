using Microsoft.AspNetCore.Mvc;

namespace Travel. Web.ViewComponents.Contact_Card
{
    public class _ContactCardsViewComponent : ViewComponent
    { 
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
