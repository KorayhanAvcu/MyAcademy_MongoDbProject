using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.AboutTeamMemberDtos;
using Travel.Web.Services.AboutTeamMemberServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AboutTeamMemberController(
        IAboutTeamMemberService aboutTeamMemberService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var values = await aboutTeamMemberService.GetAllAsync();

            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutTeamMemberCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await aboutTeamMemberService.CreateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var value = await aboutTeamMemberService.GetByIdForUpdateAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            return View(value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AboutTeamMemberUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await aboutTeamMemberService.UpdateAsync(dto);

            return RedirectToAction(nameof(Index));
        }
    }
}