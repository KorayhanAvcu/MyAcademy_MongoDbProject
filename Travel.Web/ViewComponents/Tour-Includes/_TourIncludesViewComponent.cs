using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.ViewComponents.Tour_Includes
{
    public class _TourIncludesViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(TourDetailDto model)
        {
            return View(model);
        }
    }
}
