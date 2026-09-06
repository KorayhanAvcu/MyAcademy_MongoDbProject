using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.WhyChooseUsDtos;
using Travel.Web.Services.WhyChooseUsServices;

namespace Travel.Web.ViewComponents.Default_WhyChooseUs
{
    public class _DefaultWhyChooseUsViewComponent(
        IWhyChooseUsService _whyChooseUsService,
        IMapper _mapper) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _whyChooseUsService.GetAllAsync();

            var result = _mapper.Map<List<ResultWhyChooseUsDto>>(values);

            result = result
                .Where(x => x.IsActive)
                .OrderBy(x => x.Order)
                .ToList();

            return View(result);
        }
    }
}