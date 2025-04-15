using System.Reflection;
using System.Transactions;
using _2;
using Npgsql;

namespace _3;

public partial class AppDbContext(string connectionString)
{
    private readonly ChangeTracker _changeTracker = new();
    private readonly string _connectionString = connectionString;
    public IReadOnlyList<EntityEntry> GetTrackedEntities() => _changeTracker.GetTrackedEntities();
    public void DetectChanges() => _changeTracker.DetectChanges();

    public async Task CreateTableIfNotExistsAsync<T>() where T : class
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();

        var tableName = GetTableName<T>();

        // Build column definitions
        var props = typeof(T).GetProperties();
        var columnDefinitions = new List<string>();

        foreach (var prop in props)
        {
            var columnName = prop.Name.ToLower();
            var columnType = GetSqlType(prop.PropertyType);

            if (columnName == "id" && prop.PropertyType == typeof(int))
                columnDefinitions.Add($"{columnName} SERIAL PRIMARY KEY");
            else
                columnDefinitions.Add($"{columnName} {columnType}");
        }

        var columns = string.Join(", ", columnDefinitions);

        var sql = $"""
        CREATE TABLE IF NOT EXISTS {tableName} (
            {columns}
        );
    """;

        var command = new NpgsqlCommand(sql, connection);

        await command.ExecuteNonQueryAsync();
    }

    private NpgsqlConnection CreateConnection() =>
        new NpgsqlConnection(_connectionString);

    private static string GetTableName<T>() =>
        typeof(T).Name.ToLower() + "s";

    private string GetTableName(Type type) => 
        type.Name.ToLower() + "s";

    private static List<PropertyInfo> GetPropertiesWithoutId<T>() =>
        typeof(T).GetProperties()
            .Where(p => !string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase))
            .ToList();

    private IEnumerable<PropertyInfo> GetPropertiesWithoutId(Type type)
    {
        return type.GetProperties().Where(p => 
            !string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase));
    }

    private static string GetSqlType(Type type)
    {
        if (type == typeof(int))
            return "INTEGER";
        if (type == typeof(string))
            return "VARCHAR(255)";
        if (type == typeof(DateTime))
            return "TIMESTAMP";
        if (type == typeof(bool))
            return "BOOLEAN";

        throw new NotSupportedException($"Type '{type.Name}' is not supported!");
    }
}
