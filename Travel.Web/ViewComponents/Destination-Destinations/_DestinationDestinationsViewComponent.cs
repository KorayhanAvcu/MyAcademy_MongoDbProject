using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Destination_Destinations
{
    public class _DestinationDestinationsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
