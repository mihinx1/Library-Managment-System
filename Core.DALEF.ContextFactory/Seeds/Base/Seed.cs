using Core.DAL.Infrastructure;

namespace Core.DALEF.ContextFactory.Seeds;

public class Seed<T> : List<T> where T : Entity
{
    private static readonly DateTime date = new DateTime(1800, 1, 1);
    private static readonly Guid userId = new Guid("89b87015-b687-4cad-a553-f911cf2e6fcf");

    public void AddEntity(T entity)
    {
        InitEntity(entity);
        Add(entity);
    }

    public void AddEntities(params T[] entities)
    {
        foreach (var entity in entities)
            InitEntity(entity);

        AddRange(entities);
    }

    private void InitEntity(T entity)
    {
        entity.Active = true;
        entity.DateCreated = entity.DateChanged = date;
        entity.UserCreatedID = entity.UserChangedID = userId;
    }
}
