using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.AboutStatDtos;
using Travel.Web.Services.AboutStatServices;
// Burası olmayacak 4 alan sabit kalıp datası veritabanından çekilecek
namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AboutStatController(
        IAboutStatService aboutStatService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var values = await aboutStatService.GetAllAsync();

            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutStatCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await aboutStatService.CreateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var value = await aboutStatService.GetByIdForUpdateAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            return View(value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AboutStatUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await aboutStatService.UpdateAsync(dto);

            return RedirectToAction(nameof(Index));
        }
    }
}