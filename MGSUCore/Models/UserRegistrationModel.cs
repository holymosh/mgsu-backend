using System.ComponentModel.DataAnnotations;
using Common.Entities;

namespace MGSUCore.Models
{
    public class UserRegistrationModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public UserProfile UserProfile { get; set; }
    }
}