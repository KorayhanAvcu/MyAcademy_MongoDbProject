using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Travel.Web.Entities
{
    public class AppUser : MongoIdentityUser<string>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool TermsAccepted { get; set; }

        public DateTime? TermsAcceptedAt { get; set; }
    }
}
