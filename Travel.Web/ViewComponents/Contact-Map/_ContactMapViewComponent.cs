using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Contact_Map
{
    public class _ContactMapViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    
    }
}
