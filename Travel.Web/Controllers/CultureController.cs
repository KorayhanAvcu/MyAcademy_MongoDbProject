using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Controllers
{
    public class CultureController : Controller
    {
        private static readonly string[] Supported = { "tr-TR", "en-US" };

        // Seçilen dili çereze yazar ve kullanıcıyı geldiği sayfaya döndürür.
        [HttpGet]
        public IActionResult Set(string culture, string? returnUrl)
        {
            if (!Supported.Contains(culture))
                culture = "tr-TR";

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true });

            if (string.IsNullOrWhiteSpace(returnUrl) || !Url.IsLocalUrl(returnUrl))
                returnUrl = "/";

            return LocalRedirect(returnUrl);
        }
    }
}
