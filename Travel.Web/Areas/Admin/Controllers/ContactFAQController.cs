using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.ContactFAQDtos;
using Travel.Web.Services.ContactFAQServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactFAQController(
        IContactFAQService contactFAQService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var values =
                await contactFAQService.GetAllAsync();

            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateContactFAQDto contactFAQCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(contactFAQCreateDto);
            }

            await contactFAQService.CreateAsync(
                contactFAQCreateDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var value =
                await contactFAQService.GetByIdAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateContactFAQDto
            {
                Id = value.Id,
                Question = value.Question,
                Answer = value.Answer
            };

            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(
            UpdateContactFAQDto contactFAQUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(contactFAQUpdateDto);
            }

            await contactFAQService.UpdateAsync(
                contactFAQUpdateDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await contactFAQService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}