using _3.Models;

namespace _3;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            // Setup connection string
            var connectionString = "Host=localhost;Port=5432;Database=notification;Username=postgres;Password=web@1234";

            // Initialize the database context
            Console.WriteLine("Initializing database context...");
            var context = new AppDbContext(connectionString);

            // Create tables if they don't exist
            Console.WriteLine("Ensuring database tables exist...");
            await context.CreateTableIfNotExistsAsync<Developer>();
            Console.WriteLine("Database is ready!\n");

            // -------------------------
            // CREATE Operation
            // -------------------------
            Console.WriteLine("--------- CREATE OPERATIONS ---------");

            // Create multiple developers
            var developers = new List<Developer>
            {
                new Developer { FullName = "Alex Johnson", Stack = ".NET", Position = "Senior Developer" },
                new Developer { FullName = "Maria Garcia", Stack = "Java", Position = "Lead Developer" },
                new Developer { FullName = "Sam Taylor", Stack = "JavaScript", Position = "Frontend Developer" }
            };

            Console.WriteLine("Adding developers to the tracker...");
            foreach (var dev in developers)
            {
                context.Add(dev);
                Console.WriteLine($"Tracked: {dev.FullName} - {dev.Stack}");
            }

            // Save changes to insert the new developers
            var affected = await context.SaveChangesAsync();
            Console.WriteLine($"Created {affected} new developer records\n");

            // -------------------------
            // READ Operations
            // -------------------------
            Console.WriteLine("--------- READ OPERATIONS ---------");

            // Get all developers
            Console.WriteLine("Reading all developers:");
            var allDevelopers = await context.GetAllAsync<Developer>();

            DisplayDevelopers(allDevelopers);

            // Let's save a reference to one developer for later update
            var developerToUpdate = allDevelopers.FirstOrDefault();
            // And another for deletion
            var developerToDelete = allDevelopers.LastOrDefault();

            // -------------------------
            // UPDATE Operation
            // -------------------------
            Console.WriteLine("\n--------- UPDATE OPERATIONS ---------");

            if (developerToUpdate != null)
            {
                // Update a developer's information
                Console.WriteLine($"Updating developer: {developerToUpdate.FullName}");

                // Change some properties
                developerToUpdate.Position = "Technical Lead";
                developerToUpdate.Stack = $"{developerToUpdate.Stack}, Azure";

                // Track the update
                context.Update(developerToUpdate);

                // Save changes
                affected = await context.SaveChangesAsync();
                Console.WriteLine($"Updated {affected} developer record");

                // Verify the update
                Console.WriteLine("\nVerifying update - Reading all developers again:");
                allDevelopers = await context.GetAllAsync<Developer>();
                DisplayDevelopers(allDevelopers);
            }

            // -------------------------
            // DELETE Operation
            // -------------------------
            Console.WriteLine("\n--------- DELETE OPERATIONS ---------");

            if (developerToDelete != null)
            {
                // Delete a developer
                Console.WriteLine($"Deleting developer: {developerToDelete.FullName}");

                // Track the deletion
                context.Delete(developerToDelete);

                // Save changes
                affected = await context.SaveChangesAsync();
                Console.WriteLine($"Deleted {affected} developer record");

                // Verify the deletion
                Console.WriteLine("\nVerifying deletion - Reading all developers again:");
                allDevelopers = await context.GetAllAsync<Developer>();
                DisplayDevelopers(allDevelopers);
            }

            // -------------------------
            // Combined Operations
            // -------------------------
            Console.WriteLine("\n--------- COMBINED OPERATIONS ---------");

            // Let's add a new developer, update one, and delete one all in one transaction
            Console.WriteLine("Performing multiple operations in one transaction");

            // Create a new developer
            var newDeveloper = new Developer
            {
                FullName = "Kim Lee",
                Stack = "Python, Django",
                Position = "Backend Developer"
            };

            // Add the new developer
            context.Add(newDeveloper);
            Console.WriteLine($"Tracked for creation: {newDeveloper.FullName}");

            // Update an existing developer if available
            var developerToModify = allDevelopers.FirstOrDefault();
            if (developerToModify != null)
            {
                developerToModify.Position = "Solution Architect";
                context.Update(developerToModify);
                Console.WriteLine($"Tracked for update: {developerToModify.FullName} to {developerToModify.Position}");
            }

            // Delete a developer if more than one available
            if (allDevelopers.Count > 1)
            {
                var anotherToDelete = allDevelopers[1];
                context.Delete(anotherToDelete);
                Console.WriteLine($"Tracked for deletion: {anotherToDelete.FullName}");
            }

            // Save all changes in one transaction
            affected = await context.SaveChangesAsync();
            Console.WriteLine($"Total records affected in combined operations: {affected}");

            // Final state of the database
            Console.WriteLine("\nFinal state of the database:");
            allDevelopers = await context.GetAllAsync<Developer>();
            DisplayDevelopers(allDevelopers);

            Console.WriteLine("\nCRUD operations completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }

    private static void DisplayDevelopers(List<Developer> developers)
    {
        if (developers.Count == 0)
        {
            Console.WriteLine("No developers found in database.");
            return;
        }

        Console.WriteLine($"Found {developers.Count} developers:");
        foreach (var dev in developers)
        {
            Console.WriteLine($"ID: {dev.Id} | {dev.FullName} | {dev.Stack} | {dev.Position}");
        }
    }
}
