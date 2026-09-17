using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Destination_Filter
{
    public class _DestinationFilterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
