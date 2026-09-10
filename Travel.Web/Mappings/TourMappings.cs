using AutoMapper;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Entities.Tour;

namespace Travel.Web.Mappings
{
    public class TourMappings : Profile
    {
        public TourMappings()
        {
            // Tour
            CreateMap<TourCreateDto, Tour>();
            CreateMap<TourUpdateDto, Tour>();

            // BURASI ÖNEMLİ
            CreateMap<Tour, TourUpdateDto>();

            CreateMap<Tour, TourResultDto>();
            CreateMap<Tour, TourListItemDto>();


            // TourDate
            CreateMap<TourDateDto, TourDate>();
            CreateMap<TourDate, TourDateDto>();
            CreateMap<TourDate, TourDateResultDto>();


            // DayProgram
            CreateMap<DayProgramDto, DayProgram>();
            CreateMap<DayProgram, DayProgramDto>();
        }
    }
}