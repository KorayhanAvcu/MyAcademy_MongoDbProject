using AutoMapper;
using Travel.Web.DTOs.ContactMapDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mapping
{
    public class ContactMapMapping : Profile
    {
        public ContactMapMapping()
        {
            CreateMap<ContactMap, ResultContactMapDto>();

            CreateMap<CreateContactMapDto, ContactMap>();

            CreateMap<UpdateContactMapDto, ContactMap>();

            CreateMap<ContactMap, UpdateContactMapDto>();
        }
    }
}