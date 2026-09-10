namespace Travel.Web.DTOs.TourDtos
{
    public class TourListItemDto
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? ShortDescription { get; set; }

        public string? CategoryName { get; set; }

        public string? DestinationName { get; set; }

        public string? CoverImageUrl { get; set; }

        public decimal Price { get; set; }

        public int DurationDays { get; set; }

        public int DurationNights { get; set; }

        public int MaxCapacity { get; set; }

        public int ReservationCount { get; set; }

        public DateTime? UpcomingDate { get; set; }

        public string Status { get; set; } = "Draft";
    }
}