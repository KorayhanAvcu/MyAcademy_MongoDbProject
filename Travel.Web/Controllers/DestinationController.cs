using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Controllers
{
    public class DestinationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
