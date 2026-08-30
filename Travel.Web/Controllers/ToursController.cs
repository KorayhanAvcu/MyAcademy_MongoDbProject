using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Controllers
{
    public class ToursController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TourDetail()
        {
            return View();
        }
    }
}
