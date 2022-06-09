using System.Reflection;
using DbUp;
using DbUp.Engine;

namespace API.Database; 

public static class DatabaseExtension {
    
    public static IHost MigrateDatabase<TContext>(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<TContext>>();

            logger.LogInformation("Migrating postresql database.");

            string connection = configuration.GetConnectionString("PostgresAppCon");
            
            EnsureDatabase.For.PostgresqlDatabase(connection);

            UpgradeEngine upgrader = DeployChanges.To
                .PostgresqlDatabase(connection)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                logger.LogError(result.Error, "An error occurred while migrating the postresql database");
                return host;
            }

            logger.LogInformation("Migrated postresql database.");
        }

        return host;
    }
}