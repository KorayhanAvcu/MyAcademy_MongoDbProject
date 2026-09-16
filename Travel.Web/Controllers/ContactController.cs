using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
