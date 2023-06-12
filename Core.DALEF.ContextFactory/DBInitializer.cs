namespace Core.DALEF.ContextFactory;

public class DBInitializer
{
    public static void InitilizeDatabase(Context context)
    {
        // migrate database structure
        MigrationManager.Initialize(context);

        // update data
        MigrationManager.Update(context);
    }
}
