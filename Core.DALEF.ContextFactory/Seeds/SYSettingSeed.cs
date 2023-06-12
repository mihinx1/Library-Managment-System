using Core.DAL.Models;

namespace Core.DALEF.ContextFactory.Seeds;

public class SYSettingSeed : Seed<SYSetting>
{
    public SYSettingSeed()
    {
        AddEntity(new SYSetting
        {
            Id = new Guid("D81229A0-CF63-46C0-87F4-59DB2EE37256"),
            Level = 1,
            Module = "SY",
            Name = "DataSchemaVersion",
            StringValue = "SchemaVersion.V1_0_0",
            Type = "System.String",
        });
    }
}
