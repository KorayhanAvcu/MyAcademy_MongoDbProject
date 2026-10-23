using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.QuestionServices;
using Travel.Web.Services.ReservationServices;
using Travel.Web.Services.ReviewServices;
using Travel.Web.Services.StatisticServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController(
        IStatisticService statisticService,
        IReservationService reservationService,
        IReviewService reviewService,
        IQuestionService questionService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            // İstatistiklerin çoğu MongoDB Aggregation Pipeline ile hesaplanır.
            var model = await statisticService.GetDashboardAsync();

            ViewBag.RecentReservations = (await reservationService.GetAllAsync()).Take(6).ToList();

            ViewBag.RecentReviews = (await reviewService.GetAllAsync())
                .OrderByDescending(x => x.CreatedAt).Take(3).ToList();

            ViewBag.PendingQuestions = (await questionService.GetAllAsync())
                .Where(x => !x.IsAnswered).Take(4).ToList();

            return View(model);
        }
    }
}
