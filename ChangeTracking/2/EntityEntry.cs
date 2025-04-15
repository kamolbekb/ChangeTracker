namespace _2;

public class EntityEntry
{
    public object Entity { get; }
    public EntityState State { get; set; }
    public List<PropertyEntry> Properties { get; }

    public EntityEntry(object entity, EntityState state)
    {
        throw new NotImplementedException(); // TODO: Initialize entity and property snapshot
    }

    /// <summary>
    /// Detects which properties have changed.
    /// Sets entity state to Modified if any are different from original.
    /// </summary>
    public void DetectChanges()
    {
        throw new NotImplementedException(); // TODO: Check properties for changes
    }

    /// <summary>
    /// Returns a list of all modified properties for this entity.
    /// </summary>
    public List<PropertyEntry> GetModifiedProperties() =>
        throw new NotImplementedException(); // TODO: Filter and return modified properties
}
