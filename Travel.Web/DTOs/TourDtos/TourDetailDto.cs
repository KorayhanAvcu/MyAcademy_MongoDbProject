using Travel.Web.DTOs.QuestionDtos;
using Travel.Web.DTOs.ReviewDtos;

namespace Travel.Web.DTOs.TourDtos
{
    public class TourDetailDto
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public string CategoryId { get; set; } = string.Empty;

        public string DestinationId { get; set; } = string.Empty;

        public string? RouteId { get; set; }

        public string? CategoryName { get; set; }

        public string? DestinationName { get; set; }

        public string? RouteName { get; set; }

        public string? TourType { get; set; }

        public decimal Price { get; set; }

        public int DurationDays { get; set; }

        public int DurationNights { get; set; }

        public int MaxCapacity { get; set; }

        public int MinParticipants { get; set; }

        public string? DepartureCity { get; set; }

        public string? Transportation { get; set; }

        public string? Accommodation { get; set; }

        public string? GuideLanguage { get; set; }

        public string? VisaInfo { get; set; }

        public string? CoverImageUrl { get; set; }

        public List<string> GalleryImages { get; set; } = new();

        public List<TourDateResultDto> TourDates { get; set; } = new();

        public List<DayProgramDto> DayPrograms { get; set; } = new();

        public List<string> Includes { get; set; } = new();

        public List<string> Excludes { get; set; } = new();

        public List<string> Highlights { get; set; } = new();

        public string Status { get; set; } = "Draft";

        public bool IsFeatured { get; set; }

        public bool IsBestSeller { get; set; }

        public bool IsNew { get; set; }

        public decimal AverageRating { get; set; }

        public int ReviewCount { get; set; }

        public int ReservationCount { get; set; }

        // ---- Detay sayfası için hesaplanan alanlar ----

        public string? CountryName { get; set; }

        public string? CityName { get; set; }

        public List<ReviewListDto> Reviews { get; set; } = new();

        /// <summary>Index 0 = 1 yıldız ... Index 4 = 5 yıldız (yüzde)</summary>
        public int[] RatingDistribution { get; set; } = new int[5];

        public List<QuestionListDto> Questions { get; set; } = new();

        public List<TourListItemDto> SimilarTours { get; set; } = new();

        public bool IsFavorite { get; set; }

        public bool HasReviewed { get; set; }

        public decimal ChildPrice { get; set; }

        /// <summary>Gün gün programdaki şehirlerden oluşan rota.</summary>
        public string RouteText => string.Join(" · ", DayPrograms
            .Select(x => x.City)
            .Where(x => !string.IsNullOrWhiteSpace(x) && x != "—" && x != "-")
            .Distinct());
    }
}

