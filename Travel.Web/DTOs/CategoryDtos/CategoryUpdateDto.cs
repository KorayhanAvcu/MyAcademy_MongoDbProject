namespace Travel.Web.DTOs.CategoryDtos
{
    public class CategoryUpdateDto
    {
        public string Id { get; set; } 

        public string Name { get; set; } 

        public string Description { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }
    }
}
