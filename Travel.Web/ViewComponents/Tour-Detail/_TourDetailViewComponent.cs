using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.ViewComponents.Tour_Detail
{
    public class _TourDetailViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(TourDetailDto model)
        {
            return View(model);
        }
    }
}