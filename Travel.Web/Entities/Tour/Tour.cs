using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Travel.Web.Entities.Common;
using Travel.Web.Entities.Enums;

namespace Travel.Web.Entities.Tour
{
    public class Tour : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string ShortDescription { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // İngilizce içerik (çoklu dil). Boşsa Türkçe alanlar kullanılır.
        public string? NameEn { get; set; }

        public string? ShortDescriptionEn { get; set; }

        public string? DescriptionEn { get; set; }


        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; } = default!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string DestinationId { get; set; } = default!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string? RouteId { get; set; }


        public string? TourType { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Price { get; set; }

        public int DurationDays { get; set; }

        public int DurationNights { get; set; }

        public int MaxCapacity { get; set; }

        public int MinParticipants { get; set; }


        public string? DepartureCity { get; set; }

        public string? Transportation { get; set; }

        public string? Accommodation { get; set; }

        public string? GuideLanguage { get; set; }

        public string? VisaInfo { get; set; }


        public string? CoverImageUrl { get; set; }

        public List<string> GalleryImages { get; set; } = new();


        // Embedded / Nested
        public List<TourDate> TourDates { get; set; } = new();

        public List<DayProgram> DayPrograms { get; set; } = new();


        // Daha sonra çoklu dil yapısına dönüştürülebilir
        public List<string> Includes { get; set; } = new();

        public List<string> Excludes { get; set; } = new();

        public List<string> Highlights { get; set; } = new();


        public TourStatus Status { get; set; } = TourStatus.Draft;

        public bool IsFeatured { get; set; }

        public bool IsBestSeller { get; set; }

        public bool IsNew { get; set; }


        // Denormalized
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public double AverageRating { get; set; }

        public int ReviewCount { get; set; }

        public int ReservationCount { get; set; }
    }
}