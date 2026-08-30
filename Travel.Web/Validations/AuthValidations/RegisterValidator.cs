
using FluentValidation;
using Travel.Web.DTOs.AuthDtos;

namespace Travel.Web.Validators.Account
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("Ad alanı boş bırakılamaz.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Soyad alanı boş bırakılamaz.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("E-posta alanı boş bırakılamaz.")
                .EmailAddress()
                .WithMessage("Geçerli bir e-posta adresi giriniz.");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Telefon numarası boş bırakılamaz.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .WithMessage("Şifre en az 8 karakter olmalıdır.")
                .Matches("[A-Z]")
                .WithMessage("Şifre en az bir büyük harf içermelidir.")
                .Matches("[0-9]")
                .WithMessage("Şifre en az bir rakam içermelidir.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Şifreler eşleşmiyor.");

            RuleFor(x => x.TermsAccepted)
                .Equal(true)
                .WithMessage("Üyelik sözleşmesini kabul etmelisiniz.");
        }
    }
}

