using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Controllers
{
    public class ToursController(
        ITourService tourService,
        IMapper mapper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tours = await tourService.GetAllAsync();

            var values = mapper.Map<List<TourListItemDto>>(tours);

            foreach (var dto in values)
            {
                var tour = tours.FirstOrDefault(x => x.Id == dto.Id);

                if (tour == null)
                {
                    continue;
                }

                dto.UpcomingDate = tour.TourDates
                    .Where(x => x.StartDate >= DateTime.Today)
                    .OrderBy(x => x.StartDate)
                    .Select(x => (DateTime?)x.StartDate)
                    .FirstOrDefault();
            }

            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> TourDetail(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var tour = await tourService.GetByIdAsync(id);

            if (tour == null)
            {
                return NotFound();
            }

            var value = mapper.Map<TourDetailDto>(tour);

            return View(value);
        }
    }
}