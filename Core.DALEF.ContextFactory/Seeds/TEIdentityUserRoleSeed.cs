using Microsoft.AspNetCore.Identity;

namespace Core.DALEF.ContextFactory.Seeds
{
    public class TEIdentityUserRoleSeed : List<IdentityUserRole<Guid>>
    {
        public TEIdentityUserRoleSeed()
        {
            Add(new() //Admin
            {
                UserId = new Guid("a4455c52-cd74-4230-b647-30c717f4d164"),
                RoleId = new Guid("fcb01c05-f06b-4a3e-b08d-34540ac91b22")
            });
            Add(new() // Member
            {
                UserId = new Guid("ddcb65c5-3170-48be-bc9a-7bc89f741286"),
                RoleId = new Guid("ed57c3a7-d092-4c10-b608-23d66a63c261")
            });
        }
    }
}
