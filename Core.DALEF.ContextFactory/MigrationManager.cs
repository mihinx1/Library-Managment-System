using Microsoft.EntityFrameworkCore;

namespace Core.DALEF.ContextFactory;

internal class MigrationManager
{
    public static void Initialize(Context ctx)
    {
        ctx.Database.Migrate();
    }

    /// <summary>
    /// Update database values as required for the current version. Typically this is used to apply bug fixes
    /// that require updates to database settings (such as media and UI templates).
    /// </summary>
    /// <param name="context">Context to be used for updating data.</param>
    /// <remarks>This function detects the current app schema version as defined in the SYSetting table and applies
    /// all relevant updates to bring it up to the current version. By the time this method exits, the app schema version
    /// in the SYSetting table will match the current schema version as defined in <see cref="SYSetting" />.
    /// </remarks>
    /// <exception cref="Exception"></exception>
    public static void Update(Context ctx)
    {
        var curSchema = GetCurrentSchema(ctx);
        var schemaToUpgrade = Context.DataSchemaVersion;

        if (curSchema == schemaToUpgrade) return;

        var curSchemaNr = GetSchemaNumber(curSchema);
        var schemaToUpgradeNr = GetSchemaNumber(Context.DataSchemaVersion);
        while (curSchemaNr < schemaToUpgradeNr)
        {
            var updateSchema = GetSchemaVersion(curSchemaNr);
            UpgradeSchema(ctx, updateSchema);
            curSchema = GetCurrentSchema(ctx);
            curSchemaNr = GetSchemaNumber(curSchema);
        }

        if (curSchema != schemaToUpgrade)
        {
            throw new Exception($"The migration function for schema {curSchema} should have incremented the data schema version in the AppSetting table, but it did not.");
        }
    }

    // Update this method when adding newest schema (dummy or real)
    public static void UpgradeSchema(Context ctx, string schema)
    {
        switch (schema)
        {
            case "SchemaVersion.V1_0_0":
                UpgradeTo101(ctx);
                break;
            default:
                UpgradeDummy(ctx, schema);
                break;
        }
    }

    private static string GetCurrentSchema(Context context)
    {
        var ver = context.SYSetting.FirstOrDefault(a => a.Module == "SY" && a.Name == "DataSchemaVersion")?.StringValue;
        return ver ?? "SchemaVersion.V1_0_0"; // Database version check before the first seed runs
    }

    /// <summary>
    /// Convert <paramref name="version"/> to number equivalent. Example: Return 
    /// <see cref="1"/> when <paramref name="version"/> is "LRMDataSchemaVersion.V1_0_1". This is a 
    /// lookup function and does not return the current version of the database or application schema requirements.
    /// </summary>
    /// <param name="version">The version of the gallery's data schema.</param>
    /// <returns>Returns the number equivalent of the specified string.</returns>
    private static int GetSchemaNumber(string version)
    {
        if (version == null)
        {
            return -1;
        }

        var dict = GetSchemaList();

        // See whether schema list contains this version.
        if (dict.ContainsValue(version))
        {
            foreach (var item in dict)
            {
                if (item.Value == version)
                    return item.Key;
            }

        }

        return -1;
    }

    public static Dictionary<int, string> GetSchemaList()
    {
        var schemaList = new Dictionary<int, string>
            {
                // Add new schema version here.
                { 0, "SchemaVersion.V1_0_0" }
            };
        return schemaList;
    }

    private static string GetSchemaVersion(int number)
    {
        var dict = GetSchemaList();

        // See whether the schema list contains this number.
        if (dict.ContainsKey(number))
        {
            string version = dict[number];
            return version;
        }

        return string.Empty;
    }

    /// <summary>
    /// Default version update without DB upgrade.
    /// </summary>
    /// <param name="ctx"></param>
    /// <param name="version"></param>
    private static void UpgradeDummy(Context ctx, string version)
    {
        var currentS = GetSchemaNumber(version);
        currentS++;

        // Update data schema version to custom version without DB upgrade -> dont forget to update GetSchemaNumber collection
        var asDataSchema = ctx.SYSetting.First(a => a.Module == "SY" && a.Name == "DataSchemaVersion");
        asDataSchema.StringValue = GetSchemaVersion(currentS);
        ctx.SaveChanges();
    }

    /// <summary>
    /// Upgrades the 1.0.0 data to the 1.0.1 data. 
    /// Does not contain data structure changes such as new columns.
    /// </summary>
    /// <param name="ctx">Context to be used for updating data.</param>
    private static void UpgradeTo101(Context ctx)
    {
        // Change setting value - sample
        /*foreach (var metaDef in ctx.GallerySettings.Where(gs => gs.SettingName == "MediaEncoderSettings"))
        {
            metaDef.SettingValue = metaDef.SettingValue.Replace(@"-s {Width}x{Height} -b:v 384k", @"-vf ""scale=min(iw*min(640/iw\,480/ih)\,iw):min(ih*min(640/iw\,480/ih)\,ih)"" -b:v 384k");
        }*/

        // Update data schema version to 1.0.1 -> dont forget to update GetSchemaNumber collection
        var asDataSchema = ctx.SYSetting.First(a => a.Module == "SY" && a.Name == "DataSchemaVersion");
        asDataSchema.StringValue = "SchemaVersion.V1_0_1";

        ctx.SaveChanges();
    }
}
