using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.BannerDtos;
using Travel.Web.Services.BannerServices;
using Travel.Web.Services.DestinationServices;
using Travel.Web.Services.StatisticServices;

namespace Travel.Web.ViewComponents.Default_Banner
{
    public class _DefaultBannerViewComponent(
    IBannerService _bannerService,
    IStatisticService _statisticService,
    IDestinationService _destinationService,
    IMapper _mapper) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var banners = await _bannerService.GetAllAsync();

            var values = _mapper.Map<List<ResultBannerDto>>(banners);

            // Ana sayfa sayaçları: toplam tur, destinasyon ve kullanıcı sayısı
            var (tours, destinations, users) = await _statisticService.GetSiteCountersAsync();

            ViewBag.TourCount = tours;
            ViewBag.DestinationCount = destinations;
            ViewBag.UserCount = users;

            // Arama kutusu için destinasyon (ülke) listesi
            ViewBag.Countries = (await _destinationService.GetAllAsync())
                .Where(x => x.IsActive)
                .Select(x => x.Country)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            return View(values);
        }
    }
}
