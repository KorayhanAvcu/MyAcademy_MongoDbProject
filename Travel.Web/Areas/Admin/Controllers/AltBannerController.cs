using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.AltBannerDtos;
using Travel.Web.Services.AltBannerServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AltBannerController(IAltBannerService _altBannerService,
                                     IMapper _mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var altBanners = await _altBannerService.GetAllAsync();
            return View(altBanners);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAltBannerDto createAltBannerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createAltBannerDto);
            }

            await _altBannerService.CreateAsync(createAltBannerDto);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(string id)
        {
            var altBanner = await _altBannerService.GetByIdAsync(id);
            var updateAltBanner = _mapper.Map<UpdateAltBannerDto>(altBanner);

            return View(updateAltBanner);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateAltBannerDto updateAltBannerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateAltBannerDto);
            }

            await _altBannerService.UpdateAsync(updateAltBannerDto);

            return RedirectToAction(nameof(Index));
        }
    }
}