using DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Configurations;

public static class MigrationsConfiguration
{
    public static void ApplyDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GamingStoreContext>();

        while (!dbContext.CanConnect())
        {
            Thread.Sleep(5000);
        }

        dbContext.Database.Migrate();
    }

    private static bool CanConnect(this GamingStoreContext dbContext)
    {
        var connecion = dbContext.Database.GetDbConnection();
        var masterConnectionString = connecion.ConnectionString.Replace("gaming_store_db", "master");
        var masterConnection = new SqlConnection(masterConnectionString);

        try
        {
            masterConnection.Open();
            masterConnection.Close();
        }
        catch (SqlException)
        {
            return false;
        }
        finally
        {
            masterConnection?.Dispose();
        }

        return true;
    }
}
