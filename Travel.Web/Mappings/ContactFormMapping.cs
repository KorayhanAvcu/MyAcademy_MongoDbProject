using AutoMapper;
using Travel.Web.DTOs.ContactFormDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mapping
{
    public class ContactFormMapping : Profile
    {
        public ContactFormMapping()
        {
            CreateMap<ContactForm, ResultContactFormDto>();

            CreateMap<CreateContactFormDto, ContactForm>();

            CreateMap<UpdateContactFormDto, ContactForm>();

            CreateMap<ContactForm, UpdateContactFormDto>();
        }
    }
}