using Microsoft.AspNetCore.Identity;

namespace Brickwell.Models
{
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
