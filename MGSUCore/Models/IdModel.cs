using MGSUCore.Models.Convertors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace MGSUCore.Models
{
    public class IdModel
    {
        [JsonConverter(typeof(ObjectIdConverter))]
        [FromRoute]
        public string Id { get; set; }

        public static implicit operator ObjectId(IdModel model)
        {
            return ObjectId.Parse(model.Id);
        }

        public static implicit operator IdModel(ObjectId model)
        {
            return new IdModel{ Id = model.ToString() };
        }
    }
}