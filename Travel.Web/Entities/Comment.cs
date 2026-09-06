using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class Comment : BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = default!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string TourId { get; set; } = default!;

        public int Rating { get; set; }

        public string Content { get; set; } = string.Empty;

        public bool IsApproved { get; set; } = true;
    }
}
