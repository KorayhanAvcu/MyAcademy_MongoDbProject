using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.ContactMessageDtos;
using Travel.Web.Services.ContactMessageServices;

namespace Travel.Web.Controllers
{
    public class ContactFormControllers(
        IContactMessageService contactMessageService) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(
            CreateContactMessageDto contactMessageCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Lütfen zorunlu alanları doldurunuz."
                });
            }

            await contactMessageService.CreateAsync(
                contactMessageCreateDto);

            return Json(new
            {
                success = true,
                message = "Mesajınız başarıyla iletildi! En kısa sürede döneceğiz."
            });
        }
    }
}