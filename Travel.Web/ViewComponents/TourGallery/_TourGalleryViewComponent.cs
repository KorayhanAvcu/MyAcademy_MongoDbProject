using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.ViewComponents.TourGallery
{
    public class _TourGalleryViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(TourDetailDto model)
        {
            return View(model);
        }
    }
}
