using System;
using System.ComponentModel.DataAnnotations;
using MGSUCore.Controllers.Extentions;
using MGSUCore.Models.Convertors;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace MGSUCore.Models
{
    public class DonationWithRegistrationModel
    {
        
        [Required]
        [ObjectId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId ProjectId { get;  set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }
        
        [Required]        
        public string LastName { get; set; }

        [Required]        
        [EmailAddress]
        public string Email { get; set; }

        [Required]  
        public decimal Value { get; set; }

        [Required]        
        public bool Recursive { get; set; }

        [Required]
        public DateTimeOffset Date { get; set; }

        [Required]
        public bool Confirmed { get; set; }
    }
}