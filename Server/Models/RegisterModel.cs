using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class RegisterModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Profile Picture")]
        public byte[]? ProfilePicture { get; set; }

        public DateTime RegisterDate { get; set; } = DateTime.Now;

        public string? Gender { get; set; }
        public string? City { get; set; }
        public string? Designation { get; set; }

        public string? Address { get; set; }
        public string? Cell { get; set; }
    }

}