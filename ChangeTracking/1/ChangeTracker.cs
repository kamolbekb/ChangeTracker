namespace _1;

public class ChangeTracker
{
    private readonly List<EntityEntry> _trackedEntities = new();

    /// <summary>
    /// Tracks the given entity with the specified state.
    /// If the entity is already tracked, updates its state.
    /// Otherwise, adds it to the tracked list.
    /// </summary>
    /// <typeparam name="T">Type of the entity</typeparam>
    /// <param name="entity">Entity to track</param>
    /// <param name="state">State of the entity (Added, Modified, etc.)</param>
    public void Track<T>(T entity, EntityState state)
    {
        // TODO: Implement tracking logic: update state if exists, otherwise add new
    }

    /// <summary>
    /// Returns the list of all tracked entities with their states.
    /// </summary>
    public List<EntityEntry> GetTrackedEntities() => throw new NotImplementedException(); // TODO: Return list of tracked entities

    /// <summary>
    /// Clears all tracked entities.
    /// </summary>
    public void Clear() => throw new NotImplementedException(); // TODO: Clear tracked entities
}
