using _1.Models;

namespace _1;

public class Program
{
    public static void Main()
    {
        var changeTracker = new ChangeTracker();

        var person1 = new Person { Id = 1, FirstName = "Alice", Age = 25 };
        var person2 = new Person { Id = 2, FirstName = "Bob", Age = 30 };

        changeTracker.Track(person1, EntityState.Added);
        changeTracker.Track(person2, EntityState.Added);

        person1.Age = 26;
        changeTracker.Track(person1, EntityState.Modified);

        changeTracker.Track(person2, EntityState.Deleted);

        Console.WriteLine("=== Tracked Entities ===");
        foreach (var entry in changeTracker.GetTrackedEntities())
        {
            var entity = entry.Entity as Person;

            Console.WriteLine($"Entity: {entity?.FirstName}, Age: {entity?.Age}, State: {entry.State}");
        }

        changeTracker.Clear();

        Console.WriteLine("\nAfter Clear:");
        var trackedAfterClear = changeTracker.GetTrackedEntities();

        Console.WriteLine(trackedAfterClear.Count == 0
            ? "No tracked entities."
            : "Still tracking something!");

        Console.WriteLine("\nProgram finished!");
    }
}
