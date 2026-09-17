using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.AboutPageHeadDtos;
using Travel.Web.Services.AboutPageHeadServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutPageHeadController(
        IAboutPageHeadService aboutPageHeadService,
        IMapper mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var values = await aboutPageHeadService.GetAllAsync();

            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            AboutPageHeadCreateDto aboutPageHeadCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(aboutPageHeadCreateDto);
            }

            await aboutPageHeadService.CreateAsync(
                aboutPageHeadCreateDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var value = await aboutPageHeadService.GetByIdAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            var model = mapper.Map<AboutPageHeadUpdateDto>(value);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(
            AboutPageHeadUpdateDto aboutPageHeadUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(aboutPageHeadUpdateDto);
            }

            await aboutPageHeadService.UpdateAsync(
                aboutPageHeadUpdateDto);

            return RedirectToAction(nameof(Index));
        }
    }
}