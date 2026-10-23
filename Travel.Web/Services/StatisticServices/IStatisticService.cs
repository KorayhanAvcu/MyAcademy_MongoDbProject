using Travel.Web.DTOs.StatisticDtos;

namespace Travel.Web.Services.StatisticServices
{
    public interface IStatisticService
    {
        Task<DashboardDto> GetDashboardAsync();

        /// <summary>Ana sayfa sayaçları: toplam tur, destinasyon, kullanıcı.</summary>
        Task<(int Tours, int Destinations, int Users)> GetSiteCountersAsync();
    }
}
