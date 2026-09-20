using AutoMapper;
using Travel.Web.DTOs.ContactCardDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mapping
{
    public class ContactCardMapping : Profile
    {
        public ContactCardMapping()
        {
            CreateMap<ContactCard, ResultContactCardDto>();

            CreateMap<CreateContactCardDto, ContactCard>();

            CreateMap<UpdateContactCardDto, ContactCard>();

            CreateMap<ContactCard, UpdateContactCardDto>();
        }
    }
}