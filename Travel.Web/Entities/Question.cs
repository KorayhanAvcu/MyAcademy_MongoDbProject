using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class Question : BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = default!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string TourId { get; set; } = default!;

        public string QuestionText { get; set; } = string.Empty;

        public string? Answer { get; set; }

        public DateTime? AnsweredAt { get; set; }

        public bool IsAnswered { get; set; }
    }
}
