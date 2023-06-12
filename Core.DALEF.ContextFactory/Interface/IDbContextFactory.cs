namespace Core.DALEF.ContextFactory.Interface;

public interface IContextfactory
{
    Context CreateDbContext(string[] args);
}
