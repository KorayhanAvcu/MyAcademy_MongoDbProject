using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class ContactFAQ : BaseEntity
    {
        public string Question { get; set; }

        public string Answer { get; set; }
    }
}