using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    public class AppUser
    {
        // Parameterless constructor
        public AppUser() { }

        [Key]
        public int ID { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName => FirstName + " " + LastName;

        public string? Address { get; set; }
        public string? Cell { get; set; }

        [Display(Name = "Profile Picture")]
        public byte[]? ProfilePicture { get; set; }

        public string? Gender { get; set; }
        public string? City { get; set; }
        public string? Designation { get; set; }

        public int? RoleId { get; set; }
        public Role? Role { get; set; }

        public DateTime RegisterDate { get; set; } = DateTime.Now;

        public ICollection<LUT_UserRoles> UserRoles { get; set; } = new List<LUT_UserRoles>();
    }

}