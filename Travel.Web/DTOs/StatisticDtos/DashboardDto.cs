namespace Travel.Web.DTOs.StatisticDtos
{
    public class NameValueDto
    {
        public string Name { get; set; } = string.Empty;

        public double Value { get; set; }

        public string? ImageUrl { get; set; }
    }

    public class DashboardDto
    {
        public int TotalTours { get; set; }

        public int ActiveTours { get; set; }

        public int PassiveTours { get; set; }

        public int TotalReservations { get; set; }

        public int ThisMonthReservations { get; set; }

        public int TotalUsers { get; set; }

        public int TotalDestinations { get; set; }

        public int PendingQuestions { get; set; }

        public double AverageTourRating { get; set; }

        public NameValueDto? MostReservedTour { get; set; }

        public NameValueDto? TopRevenueTour { get; set; }

        public List<NameValueDto> TopTours { get; set; } = new();

        public List<NameValueDto> ToursByCategory { get; set; } = new();

        public List<NameValueDto> ReservationsByDestination { get; set; } = new();

        public List<NameValueDto> MonthlyReservations { get; set; } = new();
    }
}
