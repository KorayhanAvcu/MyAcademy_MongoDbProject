using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class AboutPageHead : BaseEntity
    {
        public string Breadcrumb { get; set; }

        public string Eyebrow { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string BackgroundImage { get; set; }
    }
}