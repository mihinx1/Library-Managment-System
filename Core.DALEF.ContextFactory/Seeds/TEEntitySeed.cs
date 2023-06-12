using IPSTemplate.Dal.Models;

namespace Core.DALEF.ContextFactory.Seeds;

internal class TEEntitySeed : Seed<TEEntity>
{
    public TEEntitySeed()
    {
        AddEntity(new TEEntity
        {
            Id = new Guid("C041F416-41E6-419E-B832-7E67C6C35FDA"),
            Attribute1 = "This is a seed record."
        });
    }
}
