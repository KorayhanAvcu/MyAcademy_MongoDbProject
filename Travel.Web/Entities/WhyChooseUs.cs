using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class WhyChooseUs : BaseEntity
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public int Order { get; set; }
    }
}
