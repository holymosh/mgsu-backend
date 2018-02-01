using System;

namespace Common.Entities
{
    public class Event : PersistentEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public Image Img { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}