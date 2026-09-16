using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.ViewComponents.Tour_Breadcrumb
{
    public class _TourBreadcrumbViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(TourDetailDto model)
        {
            return View(model);
        }
    }
}
