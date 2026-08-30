using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
