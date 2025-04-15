namespace _2;

public class ChangeTracker
{
    private readonly List<EntityEntry> _trackedEntities = new();

    /// <summary>
    /// Tracks the specified entity and its state.
    /// If already tracked, updates the state.
    /// </summary>
    public void Track<T>(T entity, EntityState state)
    {
        throw new NotImplementedException(); // TODO: Implement entity tracking logic
    }

    /// <summary>
    /// Detects property-level changes in all tracked entities.
    /// Updates their state if properties are modified.
    /// </summary>
    public void DetectChanges()
    {
        throw new NotImplementedException(); // TODO: Implement property change detection
    }

    /// <summary>
    /// Returns a list of all tracked entities and their change state.
    /// </summary>
    public List<EntityEntry> GetTrackedEntities() =>
        throw new NotImplementedException(); // TODO: Return list of tracked entities

    /// <summary>
    /// Removes all tracked entities from the tracker.
    /// </summary>
    public void Clear() =>
        throw new NotImplementedException(); // TODO: Clear internal tracking list
}
