using _2.Models;

namespace _2;

public class Program
{
    static void Main(string[] args)
    {
        var changeTracker = new ChangeTracker();

        var person = new Person { Id = 1, FirstName = "Alice", Age = 25 };

        // Entity qo'shildi
        changeTracker.Track(person, EntityState.Unchanged);

        // Property o'zgartiriladi
        person.Age = 26;

        // Endi DetectChanges orqali aniqlaymiz
        changeTracker.DetectChanges();

        foreach (var entry in changeTracker.GetTrackedEntities())
        {
            Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");

            foreach (var prop in entry.GetModifiedProperties())
            {
                Console.WriteLine($" - Property: {prop.PropertyName}, Old: {prop.OriginalValue}, New: {prop.CurrentValue}");
            }
        }

    }
}
