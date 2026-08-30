using AutoMapper;
using Travel.Web.DTOs.RouteDtos;
using route = Travel.Web.Entities.Route;

namespace Travel.Web.Mappings
{
    public class RouteMappings : Profile
    {
        public RouteMappings()
        {
            CreateMap<CreateRouteDto,route>().ReverseMap();
            CreateMap<UpdateRouteDto,route>().ReverseMap();
            CreateMap<route,ResultRouteDto>().ReverseMap();
            CreateMap<ResultRouteDto,UpdateRouteDto>().ReverseMap();

        }
    }
}
