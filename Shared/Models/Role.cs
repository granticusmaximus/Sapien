using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class Role
    {
        // Parameterless constructor
        public Role() { }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<LUT_UserRoles> UserRoles { get; set; } = new List<LUT_UserRoles>();
    }
}