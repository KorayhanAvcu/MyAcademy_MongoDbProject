using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.TourGallery
{
    public class _TourGalleryViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
