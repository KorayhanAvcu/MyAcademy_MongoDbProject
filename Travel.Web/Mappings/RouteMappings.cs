using AutoMapper;
using Travel.Web.DTOs.RouteDtos;
using route = Travel.Web.Entities.Route;

namespace Travel.Web.Mappings
{
    public class RouteMappings : Profile
    {
        public RouteMappings()
        {
            CreateMap<CreateRouteDto, Route>().ReverseMap();
            CreateMap<UpdateRouteDto, Route>().ReverseMap();
            CreateMap<Route, ResultRouteDto>().ReverseMap();
            CreateMap<UpdateRouteDto, ResultRouteDto>().ReverseMap();

        }
    }
}
