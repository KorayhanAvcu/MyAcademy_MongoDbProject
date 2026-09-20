using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.ContactCardDtos;
using Travel.Web.Services.ContactCardServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactCardController(
        IContactCardService contactCardService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var values = await contactCardService.GetAllAsync();

            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateContactCardDto contactCardCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(contactCardCreateDto);
            }

            await contactCardService.CreateAsync(
                contactCardCreateDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var value = await contactCardService.GetByIdAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateContactCardDto
            {
                Id = value.Id,
                Icon = value.Icon,
                Title = value.Title,
                Description = value.Description,
                LinkText = value.LinkText,
                LinkUrl = value.LinkUrl
            };

            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(
            UpdateContactCardDto contactCardUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(contactCardUpdateDto);
            }

            await contactCardService.UpdateAsync(
                contactCardUpdateDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await contactCardService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}