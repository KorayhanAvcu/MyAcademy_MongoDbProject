using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Destination_Stats
{
    public class _DestinationStatsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
