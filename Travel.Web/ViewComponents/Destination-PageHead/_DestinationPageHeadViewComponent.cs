using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Destination_PageHead
{
    public class _DestinationPageHeadViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
