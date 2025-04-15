using Npgsql;

namespace _3;

// Extension method to check if a column exists in the reader
public static class NpgsqlDataReaderExtensions
{
    public static bool HasColumn(this NpgsqlDataReader reader, string columnName)
    {
        for (int i = 0; i < reader.FieldCount;i++)
        {
            if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }
}
