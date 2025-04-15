using System.Reflection;

namespace _2;

public class PropertyEntry
{
    public string PropertyName { get; }
    public PropertyInfo PropertyInfo { get; }
    public object? OriginalValue { get; private set; }
    public object? CurrentValue => PropertyInfo.GetValue(Entity);
    public bool IsModified { get; private set; }
    private object Entity { get; }

    public PropertyEntry(object entity, PropertyInfo propertyInfo)
    {
        throw new NotImplementedException(); // TODO: Capture original value
    }

    /// <summary>
    /// Compares current value to original value, sets IsModified flag.
    /// </summary>
    public void CheckIfModified()
    {
        throw new NotImplementedException(); // TODO: Detect change in property value
    }

    /// <summary>
    /// Accepts the current value as the new original value.
    /// </summary>
    public void AcceptChanges()
    {
        throw new NotImplementedException(); // TODO: Reset modification tracking
    }
}

