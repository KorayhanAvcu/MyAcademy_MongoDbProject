using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Travel.Web.Entities.Common;

namespace Travel.Web.Entities
{
    public class ContactPageHead : BaseEntity
    {
        public string Breadcrumb { get; set; }
        public string Eyebrow { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}