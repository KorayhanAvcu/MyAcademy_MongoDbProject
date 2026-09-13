
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

            // Tour -> Update DTO
            // Admin güncelleme formunu doldururken kullanılıyor.
            CreateMap<Tour, TourUpdateDto>();

            // Tour -> Result DTO
            CreateMap<Tour, TourResultDto>();

            // Tour -> List DTO
            CreateMap<Tour, TourListItemDto>();

            // Tour -> Detail DTO
            CreateMap<Tour, TourDetailDto>();


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

