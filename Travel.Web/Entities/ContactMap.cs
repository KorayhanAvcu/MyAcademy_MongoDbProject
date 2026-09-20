using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class ContactMap : BaseEntity
    {
        public string Title { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public string MapUrl { get; set; }
    }
}