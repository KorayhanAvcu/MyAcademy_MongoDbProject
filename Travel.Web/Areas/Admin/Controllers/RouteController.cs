using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.RouteDtos;
using Travel.Web.Services.RouteServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RouteController(IRouteService _routeService,
                                 IMapper _mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var routes = await _routeService.GetAllAsync();
            return View(routes);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string city)
        {
            var routes = await _routeService.GetAllAsync();
            if (string.IsNullOrEmpty(city))
            {
                return View(routes);
            }
            var routesByCity = await _routeService.GetAllByCityAsync(city);
            return View(routesByCity);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRouteDto routeDto)
        {
            if (!ModelState.IsValid)
            {
                return View(routeDto);
            }
            await _routeService.CreateAsync(routeDto);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(string id)
        {
            var route = await _routeService.GetByIdAsync(id);
            var updateRoute = _mapper.Map<UpdateRouteDto>(route);
            return View(updateRoute);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateRouteDto routeDto)
        {
            if (!ModelState.IsValid)
            {
                return View(routeDto);
            }
            await _routeService.UpdateAsync(routeDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _routeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
