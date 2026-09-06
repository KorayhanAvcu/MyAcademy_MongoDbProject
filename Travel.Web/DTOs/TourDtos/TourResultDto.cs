namespace Travel.Web.DTOs.TourDtos
{
    public class TourResultDto
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string ShortDescription { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string CategoryId { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public string DestinationId { get; set; } = string.Empty;

        public string DestinationName { get; set; } = string.Empty;

        public string? Route { get; set; }

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

        public double AverageRating { get; set; }

        public int ReviewCount { get; set; }

        public int ReservationCount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}