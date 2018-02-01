using System;
using Common;
using MongoDB.Bson;

namespace Common.Entities
{
    public class Donation : PersistentEntity
    {
        public ObjectId UserId { get; set; }

        public ObjectId ProjectId { get; set; }

        public decimal Value { get; set; }

        public DateTimeOffset Date { get; set; }

        public bool Recursive { get; set; }

        public bool Confirmed { get; set; }
    }
}