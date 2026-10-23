using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Travel.Web.Entities.Enums;

namespace Travel.Web.Entities.Tour
{
    public class TourDate : ISupportInitialize
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        /// <summary>
        /// Bazı eski kayıtlarda tarih kimliği "_id" yerine "Id" alanında tutulmuş.
        /// Okurken bu alan Id'ye taşınır, kaydederken yazılmaz.
        /// </summary>
        [BsonElement("Id")]
        [BsonIgnoreIfNull]
        public BsonValue? LegacyId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Capacity { get; set; }

        public int RemainingCapacity { get; set; }

        public TourDateStatus Status { get; set; } = TourDateStatus.Active;

        void ISupportInitialize.BeginInit()
        {
        }

        void ISupportInitialize.EndInit()
        {
            if (LegacyId != null && !LegacyId.IsBsonNull)
            {
                Id = LegacyId.IsObjectId ? LegacyId.AsObjectId.ToString() : LegacyId.ToString()!;
                LegacyId = null;
            }
        }
    }
}
