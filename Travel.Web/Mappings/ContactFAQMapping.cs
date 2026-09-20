using AutoMapper;
using Travel.Web.DTOs.ContactFAQDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mapping
{
    public class ContactFAQMapping : Profile
    {
        public ContactFAQMapping()
        {
            CreateMap<ContactFAQ, ResultContactFAQDto>();

            CreateMap<CreateContactFAQDto, ContactFAQ>();

            CreateMap<UpdateContactFAQDto, ContactFAQ>();

            CreateMap<ContactFAQ, UpdateContactFAQDto>();
        }
    }
}