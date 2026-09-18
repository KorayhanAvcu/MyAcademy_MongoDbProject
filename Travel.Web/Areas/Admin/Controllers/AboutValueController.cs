using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.AboutValueDtos;
using Travel.Web.Services.AboutValueServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AboutValueController(
        IAboutValueService aboutValueService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var values = await aboutValueService.GetAllAsync();

            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutValueCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await aboutValueService.CreateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var value = await aboutValueService.GetByIdForUpdateAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            return View(value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AboutValueUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await aboutValueService.UpdateAsync(dto);

            return RedirectToAction(nameof(Index));
        }
    }
}