using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.AuthDtos;
using Travel.Web.Entities;

namespace Travel.Web.Controllers
{
    public class AuthController(
    UserManager<AppUser> _userManager,
    SignInManager<AppUser> _signInManager) : Controller
    {
// =====================================================
// SIGN IN - GET
// =====================================================

    [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }


        // =====================================================
        // SIGN IN - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            // -------------------------------------------------
            // MODEL VALIDATION
            // -------------------------------------------------

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            // -------------------------------------------------
            // FIND USER
            // -------------------------------------------------

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ViewBag.ErrorMessage = "E-posta veya şifre hatalı.";

                return View(model);
            }


            // -------------------------------------------------
            // CHECK PASSWORD
            // -------------------------------------------------

            var passwordValid = await _userManager.CheckPasswordAsync(
                user,
                model.Password
            );

            if (!passwordValid)
            {
                ViewBag.ErrorMessage = "E-posta veya şifre hatalı.";

                return View(model);
            }


            // -------------------------------------------------
            // SIGN IN
            // -------------------------------------------------

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );


            // -------------------------------------------------
            // LOGIN RESULT
            // -------------------------------------------------

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ViewBag.ErrorMessage =
                        "Hesabınız geçici olarak kilitlendi.";
                }
                else if (result.IsNotAllowed)
                {
                    ViewBag.ErrorMessage =
                        "Bu hesabın giriş yapmasına izin verilmiyor.";
                }
                else if (result.RequiresTwoFactor)
                {
                    ViewBag.ErrorMessage =
                        "İki aşamalı doğrulama gerekiyor.";
                }
                else
                {
                    ViewBag.ErrorMessage =
                        "E-posta veya şifre hatalı.";
                }

                return View(model);
            }


            // -------------------------------------------------
            // ROLE BASED REDIRECT
            // -------------------------------------------------

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction(
                    "Index",
                    "Dashboard",
                    new
                    {
                        area = "Admin"
                    }
                );
            }


            return RedirectToAction(
                "Index",
                "Default"
            );
        }


        // =====================================================
        // SIGN OUT
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(
                "Index",
                "Default"
            );
        }


        // =====================================================
        // REGISTER - GET
        // =====================================================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        // =====================================================
        // REGISTER - POST
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            try
            {
                // -------------------------------------------------
                // MODEL VALIDATION
                // -------------------------------------------------

                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        errors = ModelState
                            .Values
                            .SelectMany(x => x.Errors)
                            .Select(x => x.ErrorMessage)
                    });
                }


                // -------------------------------------------------
                // TERMS CHECK
                // -------------------------------------------------

                if (!model.TermsAccepted)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message =
                            "Üyelik sözleşmesini kabul etmelisiniz."
                    });
                }


                // -------------------------------------------------
                // PASSWORD CONFIRM CHECK
                // -------------------------------------------------

                if (model.Password != model.ConfirmPassword)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Şifreler eşleşmiyor."
                    });
                }


                // -------------------------------------------------
                // EMAIL CHECK
                // -------------------------------------------------

                var existingUser =
                    await _userManager.FindByEmailAsync(model.Email);

                if (existingUser != null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message =
                            "Bu e-posta adresi zaten kayıtlı."
                    });
                }


                // -------------------------------------------------
                // CREATE USER
                // -------------------------------------------------

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


                // -------------------------------------------------
                // CREATE IDENTITY USER
                // -------------------------------------------------

                var result =
                    await _userManager.CreateAsync(
                        user,
                        model.Password
                    );


                // -------------------------------------------------
                // CREATE ERROR
                // -------------------------------------------------

                if (!result.Succeeded)
                {
                    return BadRequest(new
                    {
                        success = false,

                        errors = result.Errors
                            .Select(x => x.Description)
                    });
                }


                // -------------------------------------------------
                // DEFAULT ROLE
                // -------------------------------------------------

                var roleResult =
                    await _userManager.AddToRoleAsync(
                        user,
                        "User"
                    );


                if (!roleResult.Succeeded)
                {
                    return BadRequest(new
                    {
                        success = false,

                        errors = roleResult.Errors
                            .Select(x => x.Description)
                    });
                }


                // -------------------------------------------------
                // SIGN IN
                // -------------------------------------------------

                await _signInManager.SignInAsync(
                    user,
                    isPersistent: false
                );


                // -------------------------------------------------
                // ROLE BASED REDIRECT
                // -------------------------------------------------

                if (await _userManager.IsInRoleAsync(
                    user,
                    "Admin"))
                {
                    return RedirectToAction(
                        "Index",
                        "Dashboard",
                        new
                        {
                            area = "Admin"
                        }
                    );
                }


                return RedirectToAction(
                    "Index",
                    "Default"
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message,
                    innerException =
                        ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
    }


}
