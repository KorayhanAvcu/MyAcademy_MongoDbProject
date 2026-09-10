using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Entities.Tour;
using Travel.Web.Services.CategoryServices;
using Travel.Web.Services.DestinationServices;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TourController(
        ITourService _tourService,
        ICategoryService _categoryService,
        IDestinationService _destinationService,
        IMapper _mapper) : Controller
    {
        // GET: /Admin/Tour
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tours = await _tourService.GetAllAsync();

            var values = _mapper.Map<List<TourListItemDto>>(tours);

            return View(values);
        }

        // GET: /Admin/Tour/TourCreate
        [HttpGet]
        public async Task<IActionResult> TourCreate()
        {
            var categories = await _categoryService.GetAllAsync();
            var destinations = await _destinationService.GetAllAsync();

            ViewBag.Categories = categories;
            ViewBag.Destinations = destinations;

            return View();
        }

        // POST: /Admin/Tour/TourCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TourCreate(TourCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllAsync();
                var destinations = await _destinationService.GetAllAsync();

                ViewBag.Categories = categories;
                ViewBag.Destinations = destinations;

                return View(model);
            }

            var tour = _mapper.Map<Tour>(model);

            await _tourService.CreateAsync(tour);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Tour/TourUpdate/{id}
        [HttpGet]
        public async Task<IActionResult> TourUpdate(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var tour = await _tourService.GetByIdAsync(id);

            if (tour == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<TourUpdateDto>(tour);

            var categories = await _categoryService.GetAllAsync();
            var destinations = await _destinationService.GetAllAsync();

            ViewBag.Categories = categories;
            ViewBag.Destinations = destinations;

            return View(model);
        }

        // POST: /Admin/Tour/TourUpdate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TourUpdate(TourUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllAsync();
                var destinations = await _destinationService.GetAllAsync();

                ViewBag.Categories = categories;
                ViewBag.Destinations = destinations;

                return View(model);
            }

            var existingTour = await _tourService.GetByIdAsync(model.Id);

            if (existingTour == null)
            {
                return NotFound();
            }

            var tour = _mapper.Map<Tour>(model);

            tour.Id = existingTour.Id;

            await _tourService.UpdateAsync(tour);

            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/Tour/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var tour = await _tourService.GetByIdAsync(id);

            if (tour == null)
            {
                return NotFound();
            }

            await _tourService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}