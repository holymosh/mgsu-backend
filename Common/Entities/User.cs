using Common;
using MongoDB.Bson;

namespace Common.Entities
{
    public class User : PersistentEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        
        public string Email { get; set; }
        public Password Password { get; set; }

        public BsonDateTime CreationTime { get; set; }
        
        public string Phone { get; set; }
        public UserRole Role { get; set; }

        public bool IsConfirmed { get; set; }

        public string EmailConfirmationToken { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}