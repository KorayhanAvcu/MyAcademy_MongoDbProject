
using Travel.Web.Entities.Common;

namespace Travel.Web.Entities.About
{
    public class AboutStorySection : BaseEntity
    {
        public string Badge { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Description2 { get; set; }

        public string ImageUrl { get; set; }

        public string PrimaryButtonText { get; set; }

        public string PrimaryButtonUrl { get; set; }

        public string SecondaryButtonText { get; set; }

        public string SecondaryButtonUrl { get; set; }
    }
}
