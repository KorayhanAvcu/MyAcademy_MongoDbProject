using AutoMapper;
using Travel.Web.DTOs.ReviewDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class ReviewMappings : Profile
    {
        public ReviewMappings()
        {
            CreateMap<Review, ReviewListDto>().ReverseMap();

            CreateMap<Review, ReviewDetailDto>().ReverseMap();

            CreateMap<CreateReviewDto, Review>().ReverseMap();

            CreateMap<UpdateReviewDto, Review>().ReverseMap();

            CreateMap<ReviewListDto, UpdateReviewDto>().ReverseMap();
        }
    }
}
