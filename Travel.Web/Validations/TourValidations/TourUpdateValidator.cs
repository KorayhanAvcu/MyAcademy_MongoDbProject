using FluentValidation;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.Validations.Tour
{
    public class TourUpdateValidator : AbstractValidator<TourUpdateDto>
    {
        public TourUpdateValidator()
        {
            // -------------------------
            // ID
            // -------------------------

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Tur ID bilgisi zorunludur.");


            // -------------------------
            // BASIC INFORMATION
            // -------------------------

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Tur adı zorunludur.")
                .MaximumLength(200)
                .WithMessage("Tur adı en fazla 200 karakter olabilir.");

            RuleFor(x => x.ShortDescription)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.ShortDescription))
                .WithMessage("Kısa açıklama en fazla 500 karakter olabilir.");

            RuleFor(x => x.Description)
                .MaximumLength(5000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description))
                .WithMessage("Açıklama en fazla 5000 karakter olabilir.");


            // -------------------------
            // CATEGORY / DESTINATION
            // -------------------------

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("Kategori seçilmelidir.");

            RuleFor(x => x.DestinationId)
                .NotEmpty()
                .WithMessage("Destinasyon seçilmelidir.");


            // -------------------------
            // PRICE
            // -------------------------

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Fiyat 0'dan büyük olmalıdır.");


            // -------------------------
            // DURATION
            // -------------------------

            RuleFor(x => x.DurationDays)
                .GreaterThan(0)
                .WithMessage("Tur süresi en az 1 gün olmalıdır.");

            RuleFor(x => x.DurationNights)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Gece sayısı negatif olamaz.");

            RuleFor(x => x.DurationNights)
                .LessThanOrEqualTo(x => x.DurationDays)
                .WithMessage("Gece sayısı gün sayısından fazla olamaz.");


            // -------------------------
            // CAPACITY
            // -------------------------

            RuleFor(x => x.MaxCapacity)
                .GreaterThan(0)
                .WithMessage("Maksimum kapasite 0'dan büyük olmalıdır.");

            RuleFor(x => x.MinParticipants)
                .GreaterThan(0)
                .WithMessage("Minimum katılımcı 0'dan büyük olmalıdır.");

            RuleFor(x => x)
                .Must(x => x.MinParticipants <= x.MaxCapacity)
                .WithMessage("Minimum katılımcı maksimum kapasiteden büyük olamaz.");


            // -------------------------
            // TOUR DATES
            // -------------------------

            RuleFor(x => x.TourDates)
                .NotEmpty()
                .WithMessage("En az bir tur tarihi eklenmelidir.");

            RuleForEach(x => x.TourDates)
                .SetValidator(new TourDateValidator());


            // -------------------------
            // DAY PROGRAMS
            // -------------------------

            RuleFor(x => x.DayPrograms)
                .NotEmpty()
                .WithMessage("En az bir günlük program eklenmelidir.");

            RuleForEach(x => x.DayPrograms)
                .SetValidator(new DayProgramValidator());


            // -------------------------
            // STATUS
            // -------------------------

            RuleFor(x => x.Status)
                .Must(BeValidStatus)
                .WithMessage("Geçersiz tur durumu. Draft, Active veya Passive olmalıdır.");
        }


        private bool BeValidStatus(string status)
        {
            return status == "Draft"
                || status == "Active"
                || status == "Passive";
        }
    }
}