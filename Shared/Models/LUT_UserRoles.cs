namespace Shared.Models
{
    public class LUT_UserRoles
    {
        // Parameterless constructor
        public LUT_UserRoles() { }

        public int UserId { get; set; }
        public AppUser User { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}