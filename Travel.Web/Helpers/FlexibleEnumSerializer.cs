using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Travel.Web.Entities.Enums;

namespace Travel.Web.Helpers
{
    /// <summary>
    /// Enum alanlarını hem sayı hem metin olarak okuyabilen serializer.
    /// Eski kayıtlarda durumlar "Published", "Aktif", "Draft" gibi metinler olarak tutulmuş;
    /// yazarken her zaman sayı olarak kaydedilir.
    /// </summary>
    public class FlexibleEnumSerializer<TEnum> : StructSerializerBase<TEnum> where TEnum : struct, Enum
    {
        private readonly Dictionary<string, TEnum> _aliases;

        public FlexibleEnumSerializer(Dictionary<string, TEnum> aliases)
        {
            _aliases = new Dictionary<string, TEnum>(aliases, StringComparer.OrdinalIgnoreCase);
        }

        public override TEnum Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var reader = context.Reader;

            switch (reader.GetCurrentBsonType())
            {
                case BsonType.Int32:
                    return (TEnum)Enum.ToObject(typeof(TEnum), reader.ReadInt32());
                case BsonType.Int64:
                    return (TEnum)Enum.ToObject(typeof(TEnum), reader.ReadInt64());
                case BsonType.String:
                    var text = reader.ReadString();
                    if (Enum.TryParse<TEnum>(text, true, out var parsed)) return parsed;
                    if (_aliases.TryGetValue(text, out var alias)) return alias;
                    return default;
                case BsonType.Null:
                    reader.ReadNull();
                    return default;
                default:
                    reader.SkipValue();
                    return default;
            }
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TEnum value)
        {
            context.Writer.WriteInt32(Convert.ToInt32(value));
        }
    }

    public static class EnumSerializerRegistration
    {
        public static readonly Dictionary<string, TourStatus> TourStatusAliases = new()
        {
            ["Published"] = TourStatus.Active,
            ["Aktif"] = TourStatus.Active,
            ["Yayında"] = TourStatus.Active,
            ["Pasif"] = TourStatus.Passive,
            ["Inactive"] = TourStatus.Passive,
            ["Taslak"] = TourStatus.Draft
        };

        public static void Register()
        {
            BsonSerializer.RegisterSerializer(new FlexibleEnumSerializer<TourStatus>(TourStatusAliases));

            BsonSerializer.RegisterSerializer(new FlexibleEnumSerializer<TourDateStatus>(new()
            {
                ["Aktif"] = TourDateStatus.Active,
                ["Pasif"] = TourDateStatus.Passive
            }));

            BsonSerializer.RegisterSerializer(new FlexibleEnumSerializer<ReservationStatus>(new()
            {
                ["Bekliyor"] = ReservationStatus.Pending,
                ["Onaylandı"] = ReservationStatus.Approved,
                ["İptal"] = ReservationStatus.Cancelled
            }));
        }
    }
}
