using System;
using System.ComponentModel.DataAnnotations;
using MGSUCore.Controllers.Extentions;
using MGSUCore.Models.Convertors;
using MongoDB.Bson;
using Newtonsoft.Json;
using BsonDateTime = MongoDB.Bson.BsonDateTime;

namespace MGSUCore.Models
{
    public class ProjectModel
    {
		[JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }

        [Required]
		public string Name { get; set; }

        [Required]
		public string Direction { get; set; }

        [Required]
        public decimal Need { get; set; }

		public decimal Given { get; set; }

		[Required]
		[MaxLength(60)]
		public string ShortDescription { get; set; }

		public string Content { get; set; }

        public ImageModel Img { get; set; }

		public bool Public { get; set; }

		public DateTime CreatingDate { get; set; }
	}
}
