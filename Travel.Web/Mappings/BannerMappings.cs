using AutoMapper;
using Travel.Web.DTOs.BannerDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class BannerMappings : Profile
    {
        public BannerMappings()
        {
            CreateMap<Banner, ResultBannerDto>().ReverseMap();

            CreateMap<Banner, UpdateBannerDto>().ReverseMap();

            CreateMap<CreateBannerDto, Banner>().ReverseMap();

            CreateMap<ResultBannerDto, UpdateBannerDto>().ReverseMap();
        }
    }
}