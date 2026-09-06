using AutoMapper;
using Travel.Web.DTOs.CategoryDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class CategoryMappings : Profile
    {
        public CategoryMappings()
        {
            CreateMap<Category, CategoryListItemDto>();
            CreateMap<Category, CategoryUpdateDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
        }
    }
}
