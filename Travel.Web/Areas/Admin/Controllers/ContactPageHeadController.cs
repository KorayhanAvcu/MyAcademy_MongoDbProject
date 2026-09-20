using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.ContactPageHeadDtos;
using Travel.Web.Services.ContactPageHeadServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactPageHeadController(
        IContactPageHeadService contactPageHeadService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var values = await contactPageHeadService.GetAllAsync();

            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateContactPageHeadDto createContactPageHeadDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createContactPageHeadDto);
            }

            await contactPageHeadService.CreateAsync(
                createContactPageHeadDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var value = await contactPageHeadService.GetByIdAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            var model = new UpdateContactPageHeadDto
            {
                Id = value.Id,
                Breadcrumb = value.Breadcrumb,
                Eyebrow = value.Eyebrow,
                Title = value.Title,
                Description = value.Description
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(
            UpdateContactPageHeadDto updateContactPageHeadDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateContactPageHeadDto);
            }

            await contactPageHeadService.UpdateAsync(
                updateContactPageHeadDto);

            return RedirectToAction(nameof(Index));
        }
    }
}