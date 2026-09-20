using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class ContactCard : BaseEntity
    {
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
    }
}