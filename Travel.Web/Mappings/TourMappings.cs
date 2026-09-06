using AutoMapper;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Entities.Tour;

namespace Travel.Web.Mappings
{
    public class TourMappings : Profile
    {
        public TourMappings()
        {
            CreateMap<TourCreateDto, Tour>();
            CreateMap<TourUpdateDto, Tour>();
            CreateMap<Tour, TourResultDto>();
            CreateMap<Tour, TourListItemDto>();

            CreateMap<TourDateDto, TourDate>();
            CreateMap<TourDate, TourDateResultDto>();

            CreateMap<DayProgramDto, DayProgram>();
            CreateMap<DayProgram, DayProgramDto>();
        }
    }
}
