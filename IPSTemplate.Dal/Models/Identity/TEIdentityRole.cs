using Microsoft.AspNetCore.Identity;

namespace IPSTemplate.Dal.Models.Identity;

public class TEIdentityRole : IdentityRole<Guid>
{
    public TEIdentityRole()
    {
        Id = Guid.NewGuid();
    }

    public TEIdentityRole(string roleName) : this()
    {
        Name = roleName;
    }
}
