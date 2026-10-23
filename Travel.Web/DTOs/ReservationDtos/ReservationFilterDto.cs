namespace Travel.Web.DTOs.ReservationDtos
{
    public class ReservationFilterDto
    {
        public string? TourId { get; set; }

        public string? TourDateId { get; set; }

        public string? UserId { get; set; }

        public string? Status { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
