using AutoMapper;
using Travel.Web.DTOs.AboutTeamMemberDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.About;

namespace Travel.Web.Mapping
{
    public class AboutTeamMemberMapping : Profile
    {
        public AboutTeamMemberMapping()
        {
            CreateMap<AboutTeamMemberCreateDto, AboutTeamMember>();
            CreateMap<AboutTeamMemberUpdateDto, AboutTeamMember>();
            CreateMap<AboutTeamMember, AboutTeamMemberResultDto>();
            CreateMap<AboutTeamMember, AboutTeamMemberUpdateDto>();
        }
    }
}