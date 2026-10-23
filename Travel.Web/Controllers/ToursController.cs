using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Travel.Web.DTOs.QuestionDtos;
using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.DTOs.ReviewDtos;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.Enums;
using Travel.Web.Helpers;
using Travel.Web.Services.FavoriteServices;
using Travel.Web.Services.QuestionServices;
using Travel.Web.Services.ReservationServices;
using Travel.Web.Services.ReviewServices;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Controllers
{
    public class ToursController(
        ITourService tourService,
        IReviewService reviewService,
        IQuestionService questionService,
        IReservationService reservationService,
        IFavoriteService favoriteService,
        UserManager<AppUser> userManager,
        IStringLocalizer<SharedResource> localizer,
        IMapper mapper) : Controller
    {
        // =========================================================
        // LIST
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var values = await tourService.GetListItemsAsync(onlyActive: true);

            var userId = userManager.GetUserId(User);
            ViewBag.FavoriteIds = userId == null
                ? new List<string>()
                : await favoriteService.GetTourIdsByUserAsync(userId);

            return View(values);
        }


        // =========================================================
        // DETAIL
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> TourDetail(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            var tour = await tourService.GetByIdAsync(id);

            // Pasif / taslak turlar yalnızca admin tarafından önizlenebilir.
            if (tour == null || (tour.Status != TourStatus.Active && !User.IsInRole("Admin")))
                return NotFound();

            tour.Localize();

            var model = mapper.Map<TourDetailDto>(tour);

            var (categoryName, country, city) =
                await tourService.GetLookupNamesAsync(tour.CategoryId, tour.DestinationId);

            model.CategoryName = categoryName;
            model.CountryName = country;
            model.CityName = city;
            model.ChildPrice = Math.Round(tour.Price * ReservationService.ChildPriceRate);

            // Yalnızca gelecekteki aktif tarihler gösterilir.
            model.TourDates = model.TourDates
                .Where(x => x.StartDate.Date >= DateTime.Today && x.Status == TourDateStatus.Active.ToString())
                .OrderBy(x => x.StartDate)
                .ToList();

            // Yorumlar + puan dağılımı
            model.Reviews = await reviewService.GetByTourAsync(id);
            model.ReviewCount = model.Reviews.Count;
            model.AverageRating = model.Reviews.Count == 0
                ? 0
                : Math.Round((decimal)model.Reviews.Average(x => x.Rating), 1);

            for (var star = 1; star <= 5; star++)
            {
                var count = model.Reviews.Count(x => x.Rating == star);
                model.RatingDistribution[star - 1] = model.Reviews.Count == 0
                    ? 0
                    : (int)Math.Round(count * 100.0 / model.Reviews.Count);
            }

            // Cevaplanmış sorular
            model.Questions = await questionService.GetAnsweredByTourAsync(id);

            // Benzer turlar: aynı kategori veya destinasyon
            var others = await tourService.GetListItemsAsync(onlyActive: true);
            model.SimilarTours = others
                .Where(x => x.Id != id)
                .OrderByDescending(x => (x.CategoryId == tour.CategoryId ? 2 : 0) + (x.DestinationId == tour.DestinationId ? 1 : 0))
                .ThenByDescending(x => x.ReservationCount)
                .Take(3)
                .ToList();

            var userId = userManager.GetUserId(User);

            if (userId != null)
            {
                model.IsFavorite = await favoriteService.IsFavoriteAsync(userId, id);
                model.HasReviewed = await reviewService.HasUserReviewedAsync(userId, id);
            }

            return View(model);
        }


        // =========================================================
        // RESERVATION
        // =========================================================

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(CreateReservationDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = FirstError();
                return RedirectToAction(nameof(TourDetail), new { id = dto.TourId });
            }

            var userId = userManager.GetUserId(User)!;
            var (success, error, _) = await reservationService.CreateAsync(userId, dto);

            if (!success)
            {
                TempData["Error"] = localizer[error!].Value;
                return RedirectToAction(nameof(TourDetail), new { id = dto.TourId });
            }

            TempData["Success"] = localizer["Rezervasyon talebin alındı. Durumunu profilinden takip edebilirsin."].Value;

            return RedirectToAction("Index", "Profile", new { tab = "trips" });
        }


        // =========================================================
        // REVIEW
        // =========================================================

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(CreateReviewDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = FirstError();
                return RedirectToAction(nameof(TourDetail), "Tours", new { id = dto.TourId }, "yorumlar");
            }

            var user = await userManager.GetUserAsync(User);

            if (await reviewService.HasUserReviewedAsync(user!.Id, dto.TourId))
            {
                TempData["Error"] = localizer["Bu tura daha önce yorum yaptın."].Value;
                return RedirectToAction(nameof(TourDetail), "Tours", new { id = dto.TourId }, "yorumlar");
            }

            dto.UserId = user.Id;
            dto.UserName = $"{user.FirstName} {user.LastName}".Trim();

            await reviewService.CreateAsync(dto);

            TempData["Success"] = localizer["Yorumun için teşekkürler!"].Value;

            return RedirectToAction(nameof(TourDetail), "Tours", new { id = dto.TourId }, "yorumlar");
        }


        // =========================================================
        // QUESTION
        // =========================================================

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AskQuestion(CreateQuestionDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = FirstError();
                return RedirectToAction(nameof(TourDetail), "Tours", new { id = dto.TourId }, "sorular");
            }

            var user = await userManager.GetUserAsync(User);

            dto.UserId = user!.Id;
            dto.UserName = $"{user.FirstName} {user.LastName}".Trim();

            await questionService.CreateAsync(dto);

            TempData["Success"] = localizer["Sorun alındı! Ekibimiz en kısa sürede yanıtlayacak."].Value;

            return RedirectToAction(nameof(TourDetail), "Tours", new { id = dto.TourId }, "sorular");
        }


        // =========================================================
        // FAVORITE (AJAX)
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFavorite(string tourId)
        {
            var userId = userManager.GetUserId(User);

            if (userId == null)
                return Unauthorized(new { loginUrl = Url.Action("SignIn", "Auth") });

            var isFavorite = await favoriteService.ToggleAsync(userId, tourId);

            return Json(new { isFavorite });
        }

        private string FirstError() =>
            ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).FirstOrDefault()
            ?? localizer["Lütfen bilgileri kontrol edin."].Value;
    }
}
