using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.AuthDtos;
using Travel.Web.Entities;

namespace Travel.Web.Controllers
{
    public class AuthController(UserManager<AppUser> _userManager,
                                SignInManager<AppUser> _signInManager) : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!model.TermsAccepted)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Üyelik sözleşmesini kabul etmelisiniz."
                });
            }
            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Bu e-posta adresi zaten kayıtlı."
                });
            }

            var user = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.Phone,

                TermsAccepted = model.TermsAccepted,
                TermsAcceptedAt = DateTime.UtcNow
            };
            var result = await _userManager.CreateAsync(
                user,
                model.Password
            );

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    success = false,
                    errors = result.Errors.Select(x => x.Description)
                });
            }

            await _userManager.AddToRoleAsync(user, "User");

            await _signInManager.SignInAsync(
                user,
                isPersistent: false
            );

            return Ok(new
            {
                success = true,
                firstName = user.FirstName
            });
        }

    }
}
