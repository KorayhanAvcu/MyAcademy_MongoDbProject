using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class ContactMessage : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
    }
}