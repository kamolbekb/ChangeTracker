using _2;
using _3.Models;
using Npgsql;

namespace _3;

public partial class AppDbContext
{
    public async Task<List<T>> GetAllAsync<T>() where T : class, new()
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();

        var tableName = GetTableName<T>();
        var properties = typeof(T).GetProperties();

        var command = new NpgsqlCommand($"SELECT * FROM {tableName}", connection);
        var reader = await command.ExecuteReaderAsync();

        var result = new List<T>();

        while (await reader.ReadAsync())
        {
            var entity = new T();

            foreach (var prop in properties)
            {
                var columnName = prop.Name.ToLower();
                if (reader.HasColumn(columnName) && !reader.IsDBNull(reader.GetOrdinal(columnName)))
                {
                    var value = reader[columnName];
                    prop.SetValue(entity, Convert.ChangeType(value, prop.PropertyType));
                }
            }

            result.Add(entity);

            _changeTracker.Track(entity, EntityState.Unchanged);
        }

        return result;
    }

    public void Add<T>(T entity) where T : class
    {
        _changeTracker.Track(entity, EntityState.Added);
    }

    public void Update<T>(T entity) where T : class, IEntity
    {
        _changeTracker.Track(entity, EntityState.Modified);
    }

    public void Delete<T>(T entity) where T : class, IEntity
    {
        _changeTracker.Track(entity, EntityState.Deleted);
    }

    public async Task<int> SaveChangesAsync()
    {
        int affectedRows = 0;
        using var connection = CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync();
        try
        {
            var trackedEntities = _changeTracker.GetTrackedEntities();

            foreach (var entry in trackedEntities)
            {
                var entity = entry.Entity;
                var state = entry.State;
                var entityType = entity.GetType();
                var tableName = GetTableName(entityType);
                var properties = GetPropertiesWithoutId(entityType);

                switch (state)
                {
                    case EntityState.Added:
                        var columns = string.Join(", ", properties.Select(p => p.Name.ToLower()));
                        var values = string.Join(", ", properties.Select(p => $"@{p.Name.ToLower()}"));

                        var insertCommand = new NpgsqlCommand(
                            $"INSERT INTO {tableName} ({columns}) VALUES ({values})", connection);

                        foreach (var prop in properties)
                        {
                            var paramName = $"@{prop.Name.ToLower()}";
                            var value = prop.GetValue(entity);
                            insertCommand.Parameters.AddWithValue(paramName, value!);
                        }

                        affectedRows += await insertCommand.ExecuteNonQueryAsync();
                        break;

                    case EntityState.Modified:
                        var setClause = string.Join(", ", properties.Select(p => $"{p.Name.ToLower()} = @{p.Name.ToLower()}"));

                        var updateCommand = new NpgsqlCommand(
                            $"UPDATE {tableName} SET {setClause} WHERE id = @id", connection);

                        foreach (var prop in properties)
                        {
                            var paramName = $"@{prop.Name.ToLower()}";
                            var value = prop.GetValue(entity);
                            updateCommand.Parameters.AddWithValue(paramName, value!);
                        }

                        var id = ((IEntity)entity).Id;
                        updateCommand.Parameters.AddWithValue("@id", id);

                        affectedRows += await updateCommand.ExecuteNonQueryAsync();
                        break;

                    case EntityState.Deleted:
                        var deleteCommand = new NpgsqlCommand(
                            $"DELETE FROM {tableName} WHERE id = @id", connection);

                        var deleteId = ((IEntity)entity).Id;
                        deleteCommand.Parameters.AddWithValue("@id", deleteId);

                        affectedRows += await deleteCommand.ExecuteNonQueryAsync();
                        break;
                }
            }

            await transaction.CommitAsync();
            _changeTracker.Clear();
            return affectedRows;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
