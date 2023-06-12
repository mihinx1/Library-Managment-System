using IPSTemplate.Dal.Models.Identity;

namespace Core.DALEF.ContextFactory.Seeds
{
    public class TEIdentityRoleSeed : List<TEIdentityRole>
    {
        public TEIdentityRoleSeed()
        {
            Add(new() //Admin
            {
                Id = new Guid("fcb01c05-f06b-4a3e-b08d-34540ac91b22"),
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                ConcurrencyStamp = "04aaa4c4-015c-47bd-84e3-f660c83eec34"
            });
            Add(new() // Member
            {
                Id = new Guid("ed57c3a7-d092-4c10-b608-23d66a63c261"),
                Name = "Member",
                NormalizedName = "MEMBER",
                ConcurrencyStamp = "47febb0e-091b-49aa-9ba0-ecf94e815713"
            });
        }
    }
}
