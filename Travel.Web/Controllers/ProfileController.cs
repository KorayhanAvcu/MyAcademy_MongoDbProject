using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Travel.Web.Entities;
using Travel.Web.Models;
using Travel.Web.Services.FavoriteServices;
using Travel.Web.Services.QuestionServices;
using Travel.Web.Services.ReservationServices;
using Travel.Web.Services.ReviewServices;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Controllers
{
    [Authorize]
    public class ProfileController(
        UserManager<AppUser> userManager,
        IReservationService reservationService,
        IFavoriteService favoriteService,
        ITourService tourService,
        IReviewService reviewService,
        IQuestionService questionService,
        IStringLocalizer<SharedResource> localizer) : Controller
    {
        // =========================================================
        // INDEX: Rezervasyonlarım, Favorilerim, Yorumlarım, Sorularım
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Index(string? tab)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("SignIn", "Auth");

            var favoriteIds = await favoriteService.GetTourIdsByUserAsync(user.Id);
            var favorites = favoriteIds.Count == 0
                ? new()
                : await tourService.GetListItemsAsync(onlyActive: false, ids: favoriteIds);

            var model = new ProfileViewModel
            {
                ActiveTab = string.IsNullOrWhiteSpace(tab) ? "overview" : tab,
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber,
                Reservations = await reservationService.GetByUserAsync(user.Id),
                // Favoriler eklenme sırasına göre
                Favorites = favoriteIds
                    .Select(id => favorites.FirstOrDefault(x => x.Id == id))
                    .Where(x => x != null)
                    .ToList()!,
                Reviews = await reviewService.GetByUserAsync(user.Id),
                Questions = await questionService.GetByUserAsync(user.Id)
            };

            return View(model);
        }


        // =========================================================
        // PROFİL BİLGİLERİ
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ProfileUpdateModel model)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("SignIn", "Auth");

            if (string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName))
            {
                TempData["Error"] = localizer["Ad ve soyad zorunludur."].Value;
                return RedirectToAction(nameof(Index), new { tab = "profile" });
            }

            user.FirstName = model.FirstName.Trim();
            user.LastName = model.LastName.Trim();
            user.PhoneNumber = model.PhoneNumber?.Trim();

            var result = await userManager.UpdateAsync(user);

            TempData[result.Succeeded ? "Success" : "Error"] = result.Succeeded
                ? localizer["Profil bilgilerin güncellendi."].Value
                : string.Join(" ", result.Errors.Select(x => x.Description));

            return RedirectToAction(nameof(Index), new { tab = "profile" });
        }


        // =========================================================
        // REZERVASYON İPTALİ
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelReservation(string id)
        {
            var userId = userManager.GetUserId(User)!;
            var (success, error) = await reservationService.CancelByUserAsync(id, userId);

            TempData[success ? "Success" : "Error"] = success
                ? localizer["Rezervasyonun iptal edildi."].Value
                : localizer[error!].Value;

            return RedirectToAction(nameof(Index), new { tab = "trips" });
        }


        // =========================================================
        // FAVORİDEN ÇIKAR
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFavorite(string tourId)
        {
            var userId = userManager.GetUserId(User)!;

            if (await favoriteService.IsFavoriteAsync(userId, tourId))
                await favoriteService.ToggleAsync(userId, tourId);

            TempData["Success"] = localizer["Favorilerden kaldırıldı."].Value;

            return RedirectToAction(nameof(Index), new { tab = "favorites" });
        }


        // =========================================================
        // YORUM SİL
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(string id)
        {
            var userId = userManager.GetUserId(User)!;
            var review = await reviewService.GetByIdAsync(id);

            // Kullanıcı yalnızca kendi yorumunu silebilir.
            if (review == null || review.UserId != userId)
                return NotFound();

            await reviewService.DeleteAsync(id);

            TempData["Success"] = localizer["Yorum silindi."].Value;

            return RedirectToAction(nameof(Index), new { tab = "reviews" });
        }
    }
}
