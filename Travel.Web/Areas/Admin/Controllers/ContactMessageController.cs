using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.ContactMessageServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactMessageController(
        IContactMessageService contactMessageService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var values = await contactMessageService.GetAllAsync();

            return View(values);
        }

        public async Task<IActionResult> Detail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            await contactMessageService.MarkAsReadAsync(id);

            var value = await contactMessageService.GetByIdAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            return View(value);
        }
    }
}