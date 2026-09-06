namespace Travel.Web.DTOs.TourDtos
{
    public class TourDateResultDto
    {
        public string Id { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Capacity { get; set; }

        public int RemainingCapacity { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}