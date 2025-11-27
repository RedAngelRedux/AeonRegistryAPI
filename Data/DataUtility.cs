namespace AeonRegistryAPI.Data;

public static class DataUtility
{
    public static string GetConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnection");
        return connectionString ?? 
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }
}
