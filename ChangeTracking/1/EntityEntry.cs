namespace _1;

public class EntityEntry
{
    public object Entity { get; }
    public EntityState State { get; set; }

    public EntityEntry(object entity, EntityState state)
    {
        Entity = entity;
        State = state;
    }
}
