using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Services.TourServices;

namespace Travel.Web.ViewComponents.Tour_Detail
{
    public class _TourDetailViewComponent(
        ITourService tourService,
        IMapper mapper) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Content("Tur ID bulunamadı.");
            }

            var tour = await tourService.GetByIdAsync(id);

            if (tour == null)
            {
                return Content("Tur bulunamadı.");
            }

            var value = mapper.Map<TourDetailDto>(tour);

            return View(value);
        }
    }
}