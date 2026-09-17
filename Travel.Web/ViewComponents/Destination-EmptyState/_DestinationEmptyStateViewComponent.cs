using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Destination_EmptyState
{
    public class _DestinationEmptyStateViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
