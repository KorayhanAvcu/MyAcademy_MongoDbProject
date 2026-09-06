using FluentValidation;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.Validations.Tour
{
    public class DayProgramValidator : AbstractValidator<DayProgramDto>
    {
        public DayProgramValidator()
        {
            RuleFor(x => x.DayNumber)
                .GreaterThan(0)
                .WithMessage("Gün numarası 0'dan büyük olmalıdır.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Gün başlığı zorunludur.")
                .MaximumLength(200)
                .WithMessage("Gün başlığı en fazla 200 karakter olabilir.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Gün açıklaması zorunludur.")
                .MaximumLength(2000)
                .WithMessage("Gün açıklaması en fazla 2000 karakter olabilir.");

            RuleFor(x => x.City)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.City))
                .WithMessage("Şehir adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Accommodation)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.Accommodation))
                .WithMessage("Konaklama bilgisi en fazla 200 karakter olabilir.");

            RuleFor(x => x.Transportation)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.Transportation))
                .WithMessage("Ulaşım bilgisi en fazla 200 karakter olabilir.");

            RuleFor(x => x.Meal)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.Meal))
                .WithMessage("Yemek bilgisi en fazla 200 karakter olabilir.");
        }
    }
}