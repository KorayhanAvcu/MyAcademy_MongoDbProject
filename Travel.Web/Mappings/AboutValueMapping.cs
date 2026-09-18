using AutoMapper;
using Travel.Web.DTOs.AboutValueDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.About;

namespace Travel.Web.Mapping
{
    public class AboutValueMapping : Profile
    {
        public AboutValueMapping()
        {
            CreateMap<AboutValueCreateDto, AboutValue>();
            CreateMap<AboutValueUpdateDto, AboutValue>();
            CreateMap<AboutValue, AboutValueResultDto>();
            CreateMap<AboutValue, AboutValueUpdateDto>();
        }
    }
}