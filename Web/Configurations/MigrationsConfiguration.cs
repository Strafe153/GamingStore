using Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Web.Configurations;

public static class MigrationsConfiguration
{
	public static async Task UseDatabaseMigrations(this WebApplication app)
	{
		using var scope = app.Services.CreateScope();
		using var dbContext = scope.ServiceProvider.GetRequiredService<GamingStoreContext>();

		while (!dbContext.CanConnect())
		{
			await Task.Delay(TimeSpan.FromSeconds(5));
		}

		//dbContext?.Database.Migrate();
	}

	private static bool CanConnect(this GamingStoreContext dbContext)
	{
		using var connection = dbContext.Database.GetDbConnection();
		var masterConnectionString = connection.ConnectionString.Replace("gaming_store_db", "master");
		using SqlConnection masterConnection = new(masterConnectionString);

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
