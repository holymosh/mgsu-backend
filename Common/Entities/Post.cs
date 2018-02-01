using System;
using Common;
using MongoDB.Bson;

namespace Common.Entities
{
    public class Post : PersistentEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public Image Img { get; set; }
        public string Category { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}