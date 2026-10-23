namespace Travel.Web.DTOs.ReservationDtos
{
    public class ReservationListDto
    {
        public string Id { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string TourId { get; set; } = string.Empty;

        public string TourName { get; set; } = string.Empty;

        public string? TourImage { get; set; }

        public string TourDateId { get; set; } = string.Empty;

        public DateTime TourStartDate { get; set; }

        public DateTime? TourEndDate { get; set; }

        public int AdultCount { get; set; }

        public int ChildCount { get; set; }

        public int TotalPerson { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }
    }
}
