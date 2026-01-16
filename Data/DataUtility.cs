using Npgsql;

namespace AeonRegistryAPI.Data;

public static class DataUtility
{
    public static string GetConnectionString(IConfiguration configuration)
    {
        // Try to get the connection string from configuration (usually in User Secrets or appsettings)
        var connectionString = configuration.GetConnectionString("DbConnection");

        // Check for DATABASE_URL environment variable (commonly used in cloud environments like Railway)
        var databseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

        return string.IsNullOrEmpty(databseUrl)
            ? connectionString!
            : BuildConnectionString(databseUrl);
    }

    private static string BuildConnectionString(string databaseUrl)
    {
        var databaseUri = new Uri(databaseUrl);

        var userInfo = databaseUri.UserInfo.Split(':');

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = databaseUri.Host,
            Port = databaseUri.Port,
            Username = userInfo[0],
            Password = userInfo[1],
            Database = databaseUri.LocalPath.TrimStart('/'),
            SslMode = SslMode.Prefer
        };

        return builder.ToString();
    }
}
