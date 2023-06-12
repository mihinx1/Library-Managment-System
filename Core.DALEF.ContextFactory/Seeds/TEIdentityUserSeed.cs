using IPSTemplate.Dal.Models.Identity;

namespace Core.DALEF.ContextFactory.Seeds
{
    public class TEIdentityUserSeed : List<TEIdentityUser>
    {
        public TEIdentityUserSeed()
        {
            Add(new() //Admin
            {
                Id = new Guid("a4455c52-cd74-4230-b647-30c717f4d164"),
                UserName = "Administrator",
                NormalizedUserName = "ADMINISTRATOR",
                Email = "admin@ipstemplate.org",
                NormalizedEmail = "ADMIN@IPSTEMPLATE.ORG",
                EmailConfirmed = true,
                Name = "Administrator",
                ConcurrencyStamp = "04aaa4c4-015c-47bd-84e3-f660c83eec34"
            });
            Add(new() // Member
            {
                Id = new Guid("ddcb65c5-3170-48be-bc9a-7bc89f741286"),
                UserName = "Member",
                NormalizedUserName = "MEMBER",
                Email = "member@ipstemplate.org",
                NormalizedEmail = "MEMBER@IPSTEMPLATE.ORG",
                EmailConfirmed = true,
                Name = "Member",
                ConcurrencyStamp = "47febb0e-091b-49aa-9ba0-ecf94e815713"
            });
        }
    }
}
