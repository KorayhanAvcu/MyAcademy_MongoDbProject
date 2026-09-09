using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.Entities;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController(UserManager<AppUser> _userManager) : Controller
    {
        
        [HttpGet]
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();

            return View(users);
        }


        
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            return View(user);
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AppUser model)
        {
            if (!ModelState.IsValid)
                return View(model);

            
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                return NotFound();

            // Güncellenecek alanlar
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.TermsAccepted = model.TermsAccepted;

            
            if (model.TermsAccepted && user.TermsAcceptedAt == null)
            {
                user.TermsAcceptedAt = DateTime.UtcNow;
            }
            else if (!model.TermsAccepted)
            {
                user.TermsAcceptedAt = null;
            }

            
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description
                    );
                }

                return View(model);
            }

            TempData["Success"] = "Kullanıcı başarıyla güncellendi.";

            return RedirectToAction(nameof(Index));
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";

                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                TempData["Error"] = "Kullanıcı silinemedi.";

                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Kullanıcı başarıyla silindi.";

            return RedirectToAction(nameof(Index));
        }
    }
}