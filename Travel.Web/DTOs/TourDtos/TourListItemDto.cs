namespace Travel.Web.DTOs.TourDtos
{
    public class TourListItemDto
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? CoverImageUrl { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string DestinationName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int DurationDays { get; set; }

        public int DurationNights { get; set; }

        public DateTime? UpcomingDate { get; set; }

        public int Capacity { get; set; }

        public int RemainingCapacity { get; set; }

        public int ReservationCount { get; set; }

        public double AverageRating { get; set; }

        public int ReviewCount { get; set; }

        public string Status { get; set; } = "Draft";

        public bool IsFeatured { get; set; }

        public bool IsBestSeller { get; set; }

        public bool IsNew { get; set; }
    }
}