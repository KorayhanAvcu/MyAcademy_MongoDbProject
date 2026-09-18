using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class AboutTeamMember : BaseEntity
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string MemberDescription { get; set; }
        public string ImageUrl { get; set; }
    }
}
