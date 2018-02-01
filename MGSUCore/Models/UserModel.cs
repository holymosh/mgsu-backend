using System;
using System.ComponentModel.DataAnnotations;
using Common.Entities;
using MGSUCore.Controllers.Extentions;
using MGSUCore.Models.Convertors;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace MGSUCore.Models
{
    public class UserModel
    {
        [ObjectId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public UserRole Role { get; set; }

        public DateTime CreatingDate { get; set; }

        public UserProfile UserProfile { get; set; }
    }
}