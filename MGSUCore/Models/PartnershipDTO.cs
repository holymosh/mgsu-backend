using System;
using System.ComponentModel.DataAnnotations;
using Common.Entities;
using MongoDB.Bson;

namespace MGSUCore.Models
{
    public class PartnershipDTO
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Direction { get; set; }

        public Partnership GetEntity()
        {
            return new Partnership()
            {
                CreatingDate = DateTime.Now,
                Description = Description,
                IsDeleted = false,
                Direction = Direction
            };
        }

        public Partnership GetEntity(string id)
        {
            var partnership = GetEntity();
            partnership.Id = ObjectId.Parse(id);
            return partnership;
        }

    }
}
