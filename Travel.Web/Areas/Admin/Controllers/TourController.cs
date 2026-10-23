using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Entities.Enums;
using Travel.Web.Entities.Tour;
using Travel.Web.Services.CategoryServices;
using Travel.Web.Services.DestinationServices;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TourController(
        ITourService tourService,
        ICategoryService categoryService,
        IDestinationService destinationService,
        IMapper mapper) : Controller
    {
        // =========================================================
        // INDEX
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Kategori / destinasyon adları dolu liste (filtreleme için)
            var values = await tourService.GetListItemsAsync(onlyActive: false);

            return View(values);
        }


        // =========================================================
        // CREATE - GET
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> TourCreate()
        {
            await LoadViewDataAsync();

            var model = new TourCreateDto
            {
                Status = "Draft"
            };

            return View(model);
        }


        // =========================================================
        // CREATE - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TourCreate(TourCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadViewDataAsync();

                return View(model);
            }

            var tour = mapper.Map<Tour>(model);

            await tourService.CreateAsync(tour);

            TempData["SuccessMessage"] =
                "Tur başarıyla oluşturuldu.";

            return RedirectToAction(nameof(Index));
        }


        // =========================================================
        // UPDATE - GET
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> TourUpdate(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var tour = await tourService.GetByIdAsync(id);

            if (tour == null)
            {
                return NotFound();
            }

            var model = mapper.Map<TourUpdateDto>(tour);

            await LoadViewDataAsync();

            return View(model);
        }


        // =========================================================
        // UPDATE - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TourUpdate(TourUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadViewDataAsync();

                return View(model);
            }

            var existingTour =
                await tourService.GetByIdAsync(model.Id);

            if (existingTour == null)
            {
                return NotFound();
            }

            var tour = mapper.Map<Tour>(model);

            // MongoDB'deki mevcut Id korunuyor.
            tour.Id = existingTour.Id;

            await tourService.UpdateAsync(tour);

            TempData["SuccessMessage"] =
                "Tur başarıyla güncellendi.";

            return RedirectToAction(nameof(Index));
        }


        // =========================================================
        // DELETE
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var tour = await tourService.GetByIdAsync(id);

            if (tour == null)
            {
                return NotFound();
            }

            await tourService.DeleteAsync(id);

            TempData["SuccessMessage"] =
                "Tur başarıyla silindi.";

            return RedirectToAction(nameof(Index));
        }


        // =========================================================
        // AKTİF / PASİF
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var tour = await tourService.GetByIdAsync(id);

            if (tour == null)
                return NotFound();

            var newStatus = tour.Status == TourStatus.Active ? TourStatus.Passive : TourStatus.Active;
            await tourService.SetStatusAsync(id, newStatus);

            TempData["SuccessMessage"] = newStatus == TourStatus.Active
                ? "Tur aktif hale getirildi."
                : "Tur pasif hale getirildi.";

            return RedirectToAction(nameof(Index));
        }


        // =========================================================
        // VIEW DATA
        // =========================================================

        private async Task LoadViewDataAsync()
        {
            var categories =
                await categoryService.GetAllAsync();

            var destinations =
                await destinationService.GetAllAsync();

            ViewBag.Categories = categories;
            ViewBag.Destinations = destinations;
        }
    }
}