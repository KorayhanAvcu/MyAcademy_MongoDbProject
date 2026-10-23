using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.ViewComponents.Tour_About
{
    public class _TourAboutViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(TourDetailDto model)
        {
            return View(model);
        }
    }
}
