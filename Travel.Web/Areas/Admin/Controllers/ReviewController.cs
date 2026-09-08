using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.ReviewServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReviewController (IReviewService _reviewService) : Controller
    {
       

        public async Task<IActionResult> Index()
        {
            var reviews = await _reviewService.GetAllAsync();

            return View(reviews);
        }
    }
}
