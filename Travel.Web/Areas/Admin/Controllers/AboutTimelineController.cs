using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.AboutTimelineDtos;
using Travel.Web.Services.AboutTimelineServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AboutTimelineController(
        IAboutTimelineService aboutTimelineService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var values = await aboutTimelineService.GetAllAsync();

            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutTimelineCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await aboutTimelineService.CreateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var value = await aboutTimelineService.GetByIdForUpdateAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            return View(value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AboutTimelineUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await aboutTimelineService.UpdateAsync(dto);

            return RedirectToAction(nameof(Index));
        }
    }
}