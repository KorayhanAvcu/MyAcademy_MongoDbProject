using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.ViewComponents.Tour_Similar
{
    public class _TourSimilarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(TourDetailDto model)
        {
            return View(model);
        }
    }
}
