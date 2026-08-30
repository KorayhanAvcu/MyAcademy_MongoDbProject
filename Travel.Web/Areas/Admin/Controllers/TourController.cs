using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TourController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TourCreate()
        {
            return View();
        }
    }
}
