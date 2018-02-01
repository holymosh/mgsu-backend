using System;
using MGSUCore.Models.Convertors;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace MGSUCore.Models
{
    public class ContactModel
    {    
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id {get; set;}
        public string Team { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Description { get; set; }
        public ImageModel Img { get; set; }

        public DateTime CreatingDate {get; set;}
    }
}