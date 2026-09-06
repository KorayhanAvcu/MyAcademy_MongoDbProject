using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Travel.Web.Entities.Common;
using Travel.Web.Entities.Enums;

namespace Travel.Web.Entities
{
    public class Reservation : BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = default!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string TourId { get; set; } = default!;

        public string TourDateId { get; set; } = default!;

        public DateTime TourStartDate { get; set; }

        public int AdultCount { get; set; }

        public int ChildCount { get; set; }

        public int TotalPerson { get; set; }

        public decimal TotalPrice { get; set; }

        public ReservationStatus Status { get; set; }
    }
}
