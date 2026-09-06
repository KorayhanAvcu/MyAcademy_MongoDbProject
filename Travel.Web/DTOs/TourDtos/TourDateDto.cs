namespace Travel.Web.DTOs.TourDtos
{
    public class TourDateDto
    {
        public string? Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Capacity { get; set; }
    }
}