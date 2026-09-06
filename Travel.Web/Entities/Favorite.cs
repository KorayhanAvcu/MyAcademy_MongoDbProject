using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class Favorite : BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = default!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string TourId { get; set; } = default!;
    }
}
