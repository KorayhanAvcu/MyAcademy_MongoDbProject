using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.BannerDtos;
using Travel.Web.Services.BannerServices;

namespace Travel.Web.ViewComponents.Default_Banner
{
    public class _DefaultBannerViewComponent(
    IBannerService _bannerService,
    IMapper _mapper) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var banners = await _bannerService.GetAllAsync();

            var values = _mapper.Map<List<ResultBannerDto>>(banners);

            return View(values);

            
        }
    }
}
