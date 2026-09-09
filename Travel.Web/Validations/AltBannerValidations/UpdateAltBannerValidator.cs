using FluentValidation;
using Travel.Web.DTOs.AltBannerDtos;

namespace Travel.Web.Validations.AltBannerValidations
{
    public class UpdateAltBannerValidator : AbstractValidator<UpdateAltBannerDto>
    {
        public UpdateAltBannerValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("ID boş bırakılamaz.");

            RuleFor(x => x.TopTitle)
                .NotEmpty()
                .WithMessage("Üst başlık boş bırakılamaz.")
                .MinimumLength(3)
                .WithMessage("Üst başlık en az 3 karakter olmalıdır.");

            RuleFor(x => x.MainTitle)
                .NotEmpty()
                .WithMessage("Ana başlık boş bırakılamaz.")
                .MinimumLength(3)
                .WithMessage("Ana başlık en az 3 karakter olmalıdır.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Açıklama boş bırakılamaz.")
                .MaximumLength(250)
                .WithMessage("Açıklama en fazla 250 karakter olmalıdır.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty()
                .WithMessage("Görsel Url boş bırakılamaz.");

            RuleFor(x => x.Button1Text)
                .NotEmpty()
                .WithMessage("1. buton metni boş bırakılamaz.");

            RuleFor(x => x.Button1Link)
                .NotEmpty()
                .WithMessage("1. buton linki boş bırakılamaz.");

            RuleFor(x => x.Button2Text)
                .NotEmpty()
                .WithMessage("2. buton metni boş bırakılamaz.");

            RuleFor(x => x.Button2Link)
                .NotEmpty()
                .WithMessage("2. buton linki boş bırakılamaz.");
        }
    }
}