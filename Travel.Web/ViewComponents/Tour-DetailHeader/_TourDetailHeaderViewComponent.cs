using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.ViewComponents.Tour_DetailHeader
{
    public class _TourDetailHeaderViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(TourDetailDto model)
        {
            return View(model);
        }
    }
}