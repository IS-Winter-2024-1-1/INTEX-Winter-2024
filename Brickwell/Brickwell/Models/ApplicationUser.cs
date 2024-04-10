using Microsoft.AspNetCore.Identity;

namespace Brickwell.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
