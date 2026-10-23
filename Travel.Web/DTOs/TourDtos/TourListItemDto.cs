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

        public string? CategoryId { get; set; }

        public string? DestinationId { get; set; }

        public double AverageRating { get; set; }

        public int ReviewCount { get; set; }

        public bool IsBestSeller { get; set; }

        public bool IsNew { get; set; }

        public bool IsFeatured { get; set; }

        public int RemainingCapacity { get; set; }

        public int TotalCapacity { get; set; }

        /// <summary>Gelecekteki aktif kalkışlar "yyyy-MM-dd|kalanKontenjan" — tarih/kişi filtresi için.</summary>
        public List<string> DateKeys { get; set; } = new();
    }
}