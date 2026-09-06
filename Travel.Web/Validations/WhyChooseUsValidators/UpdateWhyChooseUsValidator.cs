using FluentValidation;
using Travel.Web.DTOs.WhyChooseUsDtos;

namespace Travel.Web.Validators.WhyChooseUsValidators
{
    public class UpdateWhyChooseUsValidator : AbstractValidator<UpdateWhyChooseUsDto>
    {
        public UpdateWhyChooseUsValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id alanı boş bırakılamaz.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Başlık alanı boş bırakılamaz.")
                .MinimumLength(3)
                .WithMessage("Başlık en az 3 karakter olmalıdır.")
                .MaximumLength(100)
                .WithMessage("Başlık en fazla 100 karakter olabilir.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Açıklama alanı boş bırakılamaz.")
                .MinimumLength(10)
                .WithMessage("Açıklama en az 10 karakter olmalıdır.")
                .MaximumLength(500)
                .WithMessage("Açıklama en fazla 500 karakter olabilir.");

            RuleFor(x => x.Icon)
                .NotEmpty()
                .WithMessage("İkon alanı boş bırakılamaz.")
                .MaximumLength(100)
                .WithMessage("İkon adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Order)
                .GreaterThan(0)
                .WithMessage("Sıralama değeri 0'dan büyük olmalıdır.");
        }
    }
}