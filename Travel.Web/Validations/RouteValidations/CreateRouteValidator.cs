using FluentValidation;
using Travel.Web.DTOs.RouteDtos;

namespace Travel.Web.Validations.RouteValidations
{
    public class CreateRouteValidator : AbstractValidator<CreateRouteDto>
    {
        public CreateRouteValidator()
        {
            RuleFor(x => x.DestinationId)
                .NotEmpty()
                .WithMessage("Destinasyon seçilmelidir.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty()
                .WithMessage("Görsel Url boş bırakılamaz.");

            RuleFor(x => x.Duration)
                .NotEmpty()
                .WithMessage("Tur süresi boş bırakılamaz.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Fiyat 0'dan büyük olmalıdır.");
        }
    }
}