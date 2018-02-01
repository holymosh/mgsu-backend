using System;
using System.ComponentModel.DataAnnotations;
using MGSUCore.Controllers.Extentions;
using MGSUCore.Models.Convertors;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace MGSUCore.Models
{
    public class ExpandedDonationModel
    {
		[Required]
		[ObjectId]
		[JsonConverter(typeof(ObjectIdConverter))]
		public ObjectId Id { get; set; }

        [Required]
        public UserModel User { get; set; }

		[Required]	
		public ProjectModel Project { get; set; }

		[Required]
		public decimal Value { get; set; }

		public DateTimeOffset Date { get; set; }

		public bool Recursive { get; set; }

		public bool Confirmed { get; set; }

		public DateTime CreatingDate { get; set; }
    }
}