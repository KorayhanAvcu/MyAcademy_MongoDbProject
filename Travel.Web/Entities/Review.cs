using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class Review : BaseEntity
    {
        public string TourId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string? UserImageUrl { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }

        public bool IsRead { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}