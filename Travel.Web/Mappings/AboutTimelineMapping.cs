using AutoMapper;
using Travel.Web.DTOs.AboutTimelineDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.About;

namespace Travel.Web.Mapping
{
    public class AboutTimelineMapping : Profile
    {
        public AboutTimelineMapping()
        {
            CreateMap<AboutTimelineCreateDto, AboutTimeline>();
            CreateMap<AboutTimelineUpdateDto, AboutTimeline>();
            CreateMap<AboutTimeline, AboutTimelineResultDto>();
            CreateMap<AboutTimeline, AboutTimelineUpdateDto>();
        }
    }
}