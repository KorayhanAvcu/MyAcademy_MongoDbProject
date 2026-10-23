using AutoMapper;
using Travel.Web.DTOs.UserDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<AppUser, ResultUserDto>();
            CreateMap<AppUser, UserDetailDto>();
            CreateMap<CreateUserDto, AppUser>();
            CreateMap<UpdateUserDto, AppUser>();
        }
    }
}
