using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class AboutTimeline : BaseEntity
    {
        public string Year { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
