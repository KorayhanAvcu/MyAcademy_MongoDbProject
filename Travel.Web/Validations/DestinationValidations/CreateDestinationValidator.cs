using FluentValidation;
using Travel.Web.DTOs.DestinationDtos;

namespace Travel.Web.Validations.DestinationValidations
{
    public class CreateDestinationValidator : AbstractValidator<CreateDestinationDto>
    {
        public CreateDestinationValidator()
        {
            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage("Ülke boş bırakılamaz.")
                .MinimumLength(2)
                .WithMessage("Ülke en az 2 karakter olmalıdır.")
                .MaximumLength(100)
                .WithMessage("Ülke en fazla 100 karakter olabilir.");

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("Şehir boş bırakılamaz.")
                .MinimumLength(2)
                .WithMessage("Şehir en az 2 karakter olmalıdır.")
                .MaximumLength(100)
                .WithMessage("Şehir en fazla 100 karakter olabilir.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Açıklama en fazla 500 karakter olabilir.");

            RuleFor(x => x.ImageUrl)
                .MaximumLength(500)
                .WithMessage("Görsel URL en fazla 500 karakter olabilir.");
        }
    }
}