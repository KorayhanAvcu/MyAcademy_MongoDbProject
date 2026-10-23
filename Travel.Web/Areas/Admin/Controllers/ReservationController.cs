using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.Entities.Enums;
using Travel.Web.Services.ReportServices;
using Travel.Web.Services.ReservationServices;
using Travel.Web.Services.TourServices;
using Travel.Web.Services.UserServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReservationController(
        IReservationService reservationService,
        ITourService tourService,
        IUserService userService,
        IReportService reportService) : Controller
    {
        // Tur, tur tarihi, kullanıcı, tarih aralığı ve duruma göre filtreleme
        [HttpGet]
        public async Task<IActionResult> Index(ReservationFilterDto filter)
        {
            var values = await reservationService.GetAllAsync(filter);

            ViewBag.Filter = filter;
            ViewBag.Tours = await tourService.GetAllAsync();
            ViewBag.Users = await userService.GetAllAsync();

            return View(values);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(string id, string? returnUrl)
            => await ChangeStatus(id, ReservationStatus.Approved, "Rezervasyon onaylandı.", returnUrl);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(string id, string? returnUrl)
            => await ChangeStatus(id, ReservationStatus.Cancelled, "Rezervasyon iptal edildi, kontenjan iade edildi.", returnUrl);

        private async Task<IActionResult> ChangeStatus(string id, ReservationStatus status, string message, string? returnUrl)
        {
            var (success, error) = await reservationService.UpdateStatusAsync(id, status);

            TempData[success ? "Success" : "Error"] = success ? message : error;

            return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
                ? LocalRedirect(returnUrl)
                : RedirectToAction(nameof(Index));
        }

        // =========================================================
        // TUR KATILIMCI RAPORU (Excel / PDF) — MemoryStream ile doğrudan indirilir
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> ExportExcel(string tourId, string? tourDateId)
        {
            var (title, rows, fileName) = await BuildReportAsync(tourId, tourDateId);

            if (rows == null)
                return NotFound();

            var bytes = reportService.CreateParticipantExcel(title, rows);

            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName + ".xlsx");
        }

        [HttpGet]
        public async Task<IActionResult> ExportPdf(string tourId, string? tourDateId)
        {
            var (title, rows, fileName) = await BuildReportAsync(tourId, tourDateId);

            if (rows == null)
                return NotFound();

            var bytes = reportService.CreateParticipantPdf(title, rows);

            return File(bytes, "application/pdf", fileName + ".pdf");
        }

        private async Task<(string Title, List<ReservationListDto>? Rows, string FileName)> BuildReportAsync(
            string tourId, string? tourDateId)
        {
            var tour = await tourService.GetByIdAsync(tourId);

            if (tour == null)
                return ("", null, "");

            var rows = await reservationService.GetAllAsync(new ReservationFilterDto
            {
                TourId = tourId,
                TourDateId = tourDateId
            });

            var title = tour.Name;
            var date = tour.TourDates.FirstOrDefault(x => x.Id == tourDateId);

            if (date != null)
                title += $" · {date.StartDate:dd.MM.yyyy} - {date.EndDate:dd.MM.yyyy}";

            var safeName = string.Concat(tour.Name.Where(char.IsLetterOrDigit));
            var fileName = $"Katilimci_Raporu_{safeName}{(date != null ? "_" + date.StartDate.ToString("yyyyMMdd") : "")}";

            return (title, rows.OrderBy(x => x.TourStartDate).ThenBy(x => x.FullName).ToList(), fileName);
        }
    }
}
