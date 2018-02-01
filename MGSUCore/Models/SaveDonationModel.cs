using System;
using System.ComponentModel.DataAnnotations;
using MGSUCore.Controllers.Extentions;
using MGSUCore.Models.Convertors;
using MongoDB.Bson;
using Newtonsoft.Json;
using BsonDateTime = MongoDB.Bson.BsonDateTime;

namespace MGSUCore.Models
{
    public class SaveDonationModel
    {
        [Required]
		[ObjectId]
		[JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId UserId { get; set; }

		[Required]
		[ObjectId]		
		[JsonConverter(typeof(ObjectIdConverter))]
		public ObjectId ProjectId { get; set; }

		[Required]
		public decimal Value { get; set; }

		public DateTimeOffset Date { get; set; }

		public bool Recursive { get; set; }

		public bool Confirmed { get; set; }

		public DateTime CreatingDate { get; set; }
    }
}
