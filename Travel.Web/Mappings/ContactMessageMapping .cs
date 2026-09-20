using AutoMapper;
using Travel.Web.DTOs.ContactMessageDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mapping
{
    public class ContactMessageMapping : Profile
    {
        public ContactMessageMapping()
        {
            CreateMap<CreateContactMessageDto, ContactMessage>();
            CreateMap<ContactMessage, ResultContactMessageDto>();
        }
    }
}