using Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Web.Configurations;

public static class MigrationsConfiguration
{
    public static void UseDatabaseMigrations(this WebApplication app)
    {
        using var dbContext = app.Services.GetRequiredService<GamingStoreContext>();

        while (!dbContext.CanConnect())
        {
            Thread.Sleep(5000);
        }

        dbContext.Database.Migrate();
    }

    private static bool CanConnect(this GamingStoreContext dbContext)
    {
        /* using */ var connecion = dbContext.Database.GetDbConnection();
        var masterConnectionString = connecion.ConnectionString.Replace("gaming_store_db", "master");
        using var masterConnection = new SqlConnection(masterConnectionString);

        try
        {
            masterConnection.Open();
            masterConnection.Close();
        }
        catch
        {
            return false;
        }

        return true;
    }
}
