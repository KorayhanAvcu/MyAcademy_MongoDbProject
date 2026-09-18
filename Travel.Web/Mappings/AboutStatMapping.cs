using AutoMapper;
using Travel.Web.DTOs.AboutStatDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.About;

namespace Travel.Web.Mapping
{
    public class AboutStatMapping : Profile
    {
        public AboutStatMapping()
        {
            CreateMap<AboutStatCreateDto, AboutStat>();
            CreateMap<AboutStatUpdateDto, AboutStat>();
            CreateMap<AboutStat, AboutStatResultDto>();
            CreateMap<AboutStat, AboutStatUpdateDto>();
        }
    }
}