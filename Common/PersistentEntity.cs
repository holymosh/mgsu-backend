using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Common
{
    public abstract class PersistentEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

        public DateTime CreatingDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}