using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.ContactFormDtos;
using Travel.Web.Services.ContactFormServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactFormController(
        IContactFormService contactFormService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var values =
                await contactFormService.GetAllAsync();

            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateContactFormDto contactFormCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(contactFormCreateDto);
            }

            await contactFormService.CreateAsync(
                contactFormCreateDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var value =
                await contactFormService.GetByIdAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(
            UpdateContactFormDto contactFormUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(contactFormUpdateDto);
            }

            await contactFormService.UpdateAsync(
                contactFormUpdateDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await contactFormService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}