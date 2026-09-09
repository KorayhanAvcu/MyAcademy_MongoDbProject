using AutoMapper;
using Travel.Web.DTOs.AltBannerDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class AltBannerMappings : Profile
    {
        public AltBannerMappings()
        {
            CreateMap<AltBanner, ResultAltBannerDto>().ReverseMap();

            CreateMap<AltBanner, UpdateAltBannerDto>().ReverseMap();

            CreateMap<CreateAltBannerDto, AltBanner>().ReverseMap();

            CreateMap<ResultAltBannerDto, UpdateAltBannerDto>().ReverseMap();
        }
    }
}