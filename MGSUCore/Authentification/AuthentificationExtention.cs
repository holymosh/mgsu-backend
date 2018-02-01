using System;
using System.Linq;
using System.Security.Claims;
using MongoDB.Bson;

namespace MGSUCore.Authentification
{
    public static class AuthorizationExtensions
    {
        public static ObjectId GetId(this ClaimsPrincipal identity)
        {
            ObjectId id = ObjectId.Empty;
            if(!identity.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier &&
                                            ObjectId.TryParse(c.Value, out id)))
            {
                throw new ArgumentException("Bullshit in claims");
            }

            return id;
        }
    }
}