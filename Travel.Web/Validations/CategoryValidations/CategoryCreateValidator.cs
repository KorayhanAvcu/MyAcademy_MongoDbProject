using FluentValidation;
using Travel.Web.DTOs.CategoryDtos;

namespace Travel.Web.Validations.CategoryValidations
{
    public class CategoryCreateValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Kategori adı boş bırakılamaz.")
                .MaximumLength(100)
                .WithMessage("Kategori adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Kategori açıklaması boş bırakılamaz.")
                .MaximumLength(500)
                .WithMessage("Kategori açıklaması en fazla 500 karakter olabilir.");

            RuleFor(x => x.ImageUrl)
                .MaximumLength(500)
                .WithMessage("Görsel URL'si en fazla 500 karakter olabilir.");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Sıralama 0 veya daha büyük olmalıdır.");
        }
    }
}
