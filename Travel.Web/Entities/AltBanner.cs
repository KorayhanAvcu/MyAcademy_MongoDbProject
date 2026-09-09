using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class AltBanner : BaseEntity
    {
        public string ImageUrl { get; set; }

        public string TopTitle { get; set; }

        public string MainTitle { get; set; }

        public string Description { get; set; }

        public string Button1Text { get; set; }

        public string Button1Link { get; set; }

        public string Button2Text { get; set; }

        public string Button2Link { get; set; }
    }
}