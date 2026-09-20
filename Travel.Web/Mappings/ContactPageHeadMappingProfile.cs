using AutoMapper;
using Travel.Web.DTOs.ContactPageHeadDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class ContactPageHeadMappingProfile : Profile
    {
        public ContactPageHeadMappingProfile()
        {
            CreateMap<ContactPageHead, ResultContactPageHeadDto>();

            CreateMap<CreateContactPageHeadDto, ContactPageHead>();

            CreateMap<UpdateContactPageHeadDto, ContactPageHead>();
        }
    }
}