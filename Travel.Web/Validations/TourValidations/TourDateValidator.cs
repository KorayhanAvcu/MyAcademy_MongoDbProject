using FluentValidation;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.Validations.Tour
{
    public class TourDateValidator : AbstractValidator<TourDateDto>
    {
        public TourDateValidator()
        {
            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Başlangıç tarihi zorunludur.");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("Bitiş tarihi zorunludur.");

            RuleFor(x => x)
                .Must(x => x.EndDate >= x.StartDate)
                .WithMessage("Bitiş tarihi başlangıç tarihinden önce olamaz.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0)
                .WithMessage("Tur tarihi kapasitesi 0'dan büyük olmalıdır.");
        }
    }
}