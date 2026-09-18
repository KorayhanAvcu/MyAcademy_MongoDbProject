
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.AboutStorySectionDtos;
using Travel.Web.Services.AboutStorySectionServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AboutStorySectionController(
        IAboutStorySectionService aboutStorySectionService,
        IMapper mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var values = await aboutStorySectionService.GetAllAsync();

            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            AboutStorySectionCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await aboutStorySectionService.CreateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var value = await aboutStorySectionService.GetByIdAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            var model = mapper.Map<AboutStorySectionUpdateDto>(value);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            AboutStorySectionUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await aboutStorySectionService.UpdateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            await aboutStorySectionService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
