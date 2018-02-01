using System;
using System.ComponentModel.DataAnnotations;

namespace MGSUCore.Models
{
    public class EventSavingModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string Content { get; set; }
        
        public ImageModel Img { get; set; }

        [Required]        
        [DataType(DataType.DateTime)]
        public DateTimeOffset Date { get; set; }
    }
}