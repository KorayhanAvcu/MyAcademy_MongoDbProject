
using AutoMapper;
using Travel.Web.DTOs.AboutStorySectionDtos;
using Travel.Web.Entities.About;

namespace Travel.Web.Mapping
{
    public class AboutStorySectionMapping : Profile
    {
        public AboutStorySectionMapping()
        {
            CreateMap<AboutStorySectionCreateDto, AboutStorySection>();

            CreateMap<AboutStorySectionUpdateDto, AboutStorySection>();

            CreateMap<AboutStorySection, AboutStorySectionResultDto>();
        }
    }
}

