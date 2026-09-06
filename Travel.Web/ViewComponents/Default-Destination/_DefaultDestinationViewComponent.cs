using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.DestinationDtos;
using Travel.Web.Services.DestinationServices;

namespace Travel.Web.ViewComponents.Default_Destination
{
    public class _DefaultDestinationViewComponent(
        IDestinationService _destinationService,
        IMapper _mapper) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var destinations = await _destinationService.GetAllAsync();

            var values = _mapper.Map<List<ResultDestinationDto>>(destinations);

            // Rastgele 5 destinasyon
            var randomDestinations = values
                .OrderBy(x => Guid.NewGuid())
                .Take(4)
                .ToList();

            return View(randomDestinations);
        }
    }
}