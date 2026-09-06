using System.ComponentModel.DataAnnotations;

namespace Travel.Web.DTOs.TourDtos
{
    public class TourCreateDto
    {
        [Required(ErrorMessage = "Tur adı zorunludur.")]
        public string Name { get; set; } = string.Empty;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Kategori seçilmelidir.")]
        public string CategoryId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Destinasyon seçilmelidir.")]
        public string DestinationId { get; set; } = string.Empty;

        public string? Route { get; set; }

        public string? TourType { get; set; }

        [Range(0.01, double.MaxValue,
            ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Tur süresi en az 1 gün olmalıdır.")]
        public int DurationDays { get; set; }

        [Range(0, int.MaxValue,
            ErrorMessage = "Gece sayısı negatif olamaz.")]
        public int DurationNights { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Maksimum kapasite en az 1 olmalıdır.")]
        public int MaxCapacity { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Minimum katılımcı en az 1 olmalıdır.")]
        public int MinParticipants { get; set; }

        public string? DepartureCity { get; set; }

        public string? Transportation { get; set; }

        public string? Accommodation { get; set; }

        public string? GuideLanguage { get; set; }

        public string? VisaInfo { get; set; }

        public string? CoverImageUrl { get; set; }

        public List<string> GalleryImages { get; set; } = new();

        [MinLength(1,
            ErrorMessage = "En az bir tur tarihi eklenmelidir.")]
        public List<TourDateDto> TourDates { get; set; } = new();

        [MinLength(1,
            ErrorMessage = "En az bir günlük program eklenmelidir.")]
        public List<DayProgramDto> DayPrograms { get; set; } = new();

        public List<string> Includes { get; set; } = new();

        public List<string> Excludes { get; set; } = new();

        public List<string> Highlights { get; set; } = new();

        public string Status { get; set; } = "Draft";

        public bool IsFeatured { get; set; }

        public bool IsBestSeller { get; set; }

        public bool IsNew { get; set; }
    }
}