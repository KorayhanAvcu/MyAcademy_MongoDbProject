using AutoMapper;
using Travel.Web.DTOs.WhyChooseUsDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class WhyChooseUsMappings : Profile
    {
        public WhyChooseUsMappings()
        {
            CreateMap<WhyChooseUs, ResultWhyChooseUsDto>();

            CreateMap<CreateWhyChooseUsDto, WhyChooseUs>();

            CreateMap<UpdateWhyChooseUsDto, WhyChooseUs>();
        }
    }
}
