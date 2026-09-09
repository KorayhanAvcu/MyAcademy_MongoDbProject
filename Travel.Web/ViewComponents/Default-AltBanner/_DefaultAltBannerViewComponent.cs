using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.AltBannerDtos;
using Travel.Web.Services.AltBannerServices;

namespace Travel.Web.ViewComponents.Default_AltBanner
{
    public class _DefaultAltBannerViewComponent(
        IAltBannerService _altBannerService,
        IMapper _mapper) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var altBanners = await _altBannerService.GetAllAsync();

            var values = _mapper.Map<List<ResultAltBannerDto>>(altBanners);

            return View(values);
        }
    }
}