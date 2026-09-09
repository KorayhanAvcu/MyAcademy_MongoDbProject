using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.WhyChooseUsDtos;
using Travel.Web.Services.WhyChooseUsServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class WhyChooseUsController(
        IWhyChooseUsService _whyChooseUsService) : Controller
    {
        // GET: Admin/WhyChooseUs
        public async Task<IActionResult> Index()
        {
            var values = await _whyChooseUsService.GetAllAsync();

            return View(values);
        }

        // GET: Admin/WhyChooseUs/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/WhyChooseUs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateWhyChooseUsDto createWhyChooseUsDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createWhyChooseUsDto);
            }

            await _whyChooseUsService.CreateAsync(
                createWhyChooseUsDto);

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/WhyChooseUs/Update/id
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var value = await _whyChooseUsService.GetByIdAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateWhyChooseUsDto
            {
                Id = value.Id,
                Title = value.Title,
                Description = value.Description,
                Icon = value.Icon,
                IsActive = value.IsActive,
                Order = value.Order
            };

            return View(updateDto);
        }

        // POST: Admin/WhyChooseUs/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            UpdateWhyChooseUsDto updateWhyChooseUsDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateWhyChooseUsDto);
            }

            await _whyChooseUsService.UpdateAsync(
                updateWhyChooseUsDto);

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/WhyChooseUs/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            await _whyChooseUsService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}