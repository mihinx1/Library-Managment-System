using IPSTemplate.Dal.Models;

namespace Core.DALEF.ContextFactory.Seeds.Dummy;

internal class TEEntityDummy : Seed<TEEntity>
{
    public TEEntityDummy()
    {
        AddEntity(new TEEntity
        {
            Id = new Guid("50366DD5-49E5-4CC4-A06D-5EC6F4C0805E"),
            Attribute1 = "This is a dummy record."
        });
    }
}
