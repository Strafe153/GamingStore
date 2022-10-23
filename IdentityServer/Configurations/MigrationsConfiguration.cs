using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Configurations;

public static class MigrationsConfiguration
{
    public static void ApplyDatabaseMigrations(this WebApplication app, IConfiguration configuration)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();

        while (!CanConnect(configuration))
        {
            Thread.Sleep(5000);
        }

        dbContext.Database.Migrate();
    }

    private static bool CanConnect(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DatabaseConnection");
        var connection = new SqlConnection(connectionString);

        try
        {
            connection.Open();
            connection.Close();
        }
        catch (SqlException)
        {
            return false;
        }
        finally
        {
            connection?.Dispose();
        }

        return true;
    }
}
