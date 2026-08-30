using FluentValidation;
using Travel.Web.DTOs.RouteDtos;

namespace Travel.Web.Validations.RouteValidations
{
    public class CreateRouteValidator : AbstractValidator<CreateRouteDto>
    {
        public CreateRouteValidator()
        {
            RuleFor(x => x.City).NotEmpty().WithMessage("Şehir boş bırakılamaz.");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Ülke boş bırakılamaz.");
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Görsel Url boş bırakılamaz.");
            RuleFor(x => x.Duration).NotEmpty().WithMessage("Tur süresi boş bırakılamaz.");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Fiyat boş bırakılamaz.");
        }
    }
}
