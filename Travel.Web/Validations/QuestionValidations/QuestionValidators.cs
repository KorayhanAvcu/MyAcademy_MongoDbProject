using FluentValidation;
using Travel.Web.DTOs.QuestionDtos;

namespace Travel.Web.Validations.QuestionValidations
{
    public class CreateQuestionValidator : AbstractValidator<CreateQuestionDto>
    {
        public CreateQuestionValidator()
        {
            RuleFor(x => x.TourId)
                .NotEmpty()
                .WithMessage("Tur bilgisi bulunamadı.");

            RuleFor(x => x.QuestionText)
                .NotEmpty()
                .WithMessage("Lütfen sorunuzu yazın.")
                .MinimumLength(5)
                .WithMessage("Soru en az 5 karakter olmalıdır.")
                .MaximumLength(500)
                .WithMessage("Soru en fazla 500 karakter olabilir.");
        }
    }

    public class AnswerQuestionValidator : AbstractValidator<AnswerQuestionDto>
    {
        public AnswerQuestionValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Answer)
                .NotEmpty()
                .WithMessage("Lütfen cevap alanını doldurun.")
                .MaximumLength(1000)
                .WithMessage("Cevap en fazla 1000 karakter olabilir.");
        }
    }
}
