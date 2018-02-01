namespace Common.Entities
{
    public class Project : PersistentEntity
    {
        public string Name { get; set; }
        public string Direction { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }

        public Image Img {get; set;}

        public decimal Need { get; set; }

        public decimal Given { get; set; }

        public bool Public { get; set; }
    }
}