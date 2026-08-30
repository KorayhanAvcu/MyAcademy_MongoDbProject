using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;

namespace Travel.Web.Entities
{
    public class AppRole : MongoIdentityRole<string>
    {
    }
}
