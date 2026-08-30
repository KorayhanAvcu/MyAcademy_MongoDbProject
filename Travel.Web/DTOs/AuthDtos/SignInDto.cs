using System.ComponentModel.DataAnnotations;

namespace Travel.Web.DTOs.AuthDtos
{
    public class SignInDto
    {
        [Required(ErrorMessage = "E-posta adresi zorunludur.")][EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")] 
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifre zorunludur.")] 
        public string Password { get; set; }
    }
}
