using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.Entities.Enums;

namespace Travel.Web.Services.ReservationServices
{
    public interface IReservationService
    {
        /// <summary>
        /// Rezervasyon oluşturur, toplam ücreti hesaplar ve ilgili tarihin
        /// kontenjanını atomik olarak düşer. Başarısızsa hata mesajı döner.
        /// </summary>
        Task<(bool Success, string? Error, string? ReservationId)> CreateAsync(
            string userId, CreateReservationDto dto);

        Task<List<ReservationListDto>> GetAllAsync(ReservationFilterDto? filter = null);

        Task<List<ReservationListDto>> GetByUserAsync(string userId);

        Task<(bool Success, string? Error)> UpdateStatusAsync(string id, ReservationStatus status);

        Task<(bool Success, string? Error)> CancelByUserAsync(string id, string userId);

        decimal CalculateTotal(decimal unitPrice, int adult, int child);
    }
}
