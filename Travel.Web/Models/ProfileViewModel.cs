using Travel.Web.DTOs.QuestionDtos;
using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.DTOs.ReviewDtos;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.Models
{
    public class ProfileViewModel
    {
        public string ActiveTab { get; set; } = "overview";

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string Initials =>
            $"{(FirstName.Length > 0 ? FirstName[0] : ' ')}{(LastName.Length > 0 ? LastName[0] : ' ')}".Trim().ToUpper();

        public List<ReservationListDto> Reservations { get; set; } = new();

        public List<TourListItemDto> Favorites { get; set; } = new();

        public List<ReviewListDto> Reviews { get; set; } = new();

        public List<QuestionListDto> Questions { get; set; } = new();

        public List<ReservationListDto> UpcomingReservations => Reservations
            .Where(x => x.TourStartDate.Date >= DateTime.Today && x.Status != "Cancelled")
            .OrderBy(x => x.TourStartDate)
            .ToList();

        public List<ReservationListDto> PastReservations => Reservations
            .Where(x => x.TourStartDate.Date < DateTime.Today || x.Status == "Cancelled")
            .OrderByDescending(x => x.TourStartDate)
            .ToList();
    }

    public class ProfileUpdateModel
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }
    }
}
