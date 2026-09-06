namespace Travel.Web.DTOs.WhyChooseUsDtos
{
    public class UpdateWhyChooseUsDto
    {
        public string Id { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public int Order { get; set; }
    }
}
