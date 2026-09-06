namespace Travel.Web.DTOs.WhyChooseUsDtos
{
    public class CreateWhyChooseUsDto
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public int Order { get; set; }
    }
}
