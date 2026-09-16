using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Tour_Breadcrumb
{
    public class _TourBreadcrumbViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();

        }
    }
}
