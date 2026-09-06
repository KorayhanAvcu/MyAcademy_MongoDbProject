using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class Route : BaseEntity
    {
        public string DestinationId { get; set; } = string.Empty;

        public string Duration { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public decimal Price { get; set; }

    }
}
