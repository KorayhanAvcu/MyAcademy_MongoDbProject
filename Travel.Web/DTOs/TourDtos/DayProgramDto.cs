namespace Travel.Web.DTOs.TourDtos
{
    public class DayProgramDto
    {
        public int DayNumber { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? City { get; set; }

        public string? Accommodation { get; set; }

        public string? Transportation { get; set; }

        public string? Meal { get; set; }
    }
}