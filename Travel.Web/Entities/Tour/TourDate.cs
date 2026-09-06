using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Travel.Web.Entities.Enums;

namespace Travel.Web.Entities.Tour
{
    public class TourDate
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Capacity { get; set; }

        public int RemainingCapacity { get; set; }

        public TourDateStatus Status { get; set; } = TourDateStatus.Active;
    }
}