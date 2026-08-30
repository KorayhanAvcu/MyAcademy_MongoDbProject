using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.BannerDtos;
using Travel.Web.Services.BannerServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController(IBannerService _bannerService,
                                  IMapper _mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var banners = await _bannerService.GetAllAsync();
            return View(banners);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBannerDto createBannerDto)
        {
            if(!ModelState.IsValid)
            {
                return View(createBannerDto);
            }
            await _bannerService.CreateAsync(createBannerDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(string id)
        {
            var banner = await _bannerService.GetByIdAsync(id);
            var updateBanner = _mapper.Map<UpdateBannerDto>(banner);
            return View(updateBanner);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateBannerDto updateBannerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateBannerDto);
            }
            await _bannerService.UpdateAsync(updateBannerDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _bannerService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    } 
}
