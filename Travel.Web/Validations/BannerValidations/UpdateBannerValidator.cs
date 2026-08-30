using FluentValidation;
using Travel.Web.DTOs.BannerDtos;
namespace Travel.Web.Validations.BannerValidations
{
    public class UpdateBannerValidator : AbstractValidator<UpdateBannerDto>
    {
        public UpdateBannerValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş bırakılamaz.")
                                .MinimumLength(3).WithMessage("Başlık en az 3 karakter olmalıdır.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama boş bırakılamaz.")
                                       .MaximumLength(250).WithMessage("Açıklama en fazla 250 karakter olmalıdır.");
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Görsel Url boş bırakılamaz.");
        }
    }
}
