using AutoMapper;
using Travel.Web.DTOs.AboutPageHeadDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class AboutPageHeadMapping : Profile
    {
        public AboutPageHeadMapping()
        {
            // Entity → Result DTO
            CreateMap<AboutPageHead, AboutPageHeadResultDto>();

            // Create DTO → Entity
            CreateMap<AboutPageHeadCreateDto, AboutPageHead>();

            // Update DTO → Entity
            CreateMap<AboutPageHeadUpdateDto, AboutPageHead>();
        }
    }
}