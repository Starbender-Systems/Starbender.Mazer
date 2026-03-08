using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mazer.ServerDemo.Data;

public class ServerDemoDbContextFactory : IDesignTimeDbContextFactory<ServerDemoDbContext>
{
    public ServerDemoDbContext CreateDbContext(string[] args)
    {
        ServerDemoGlobalFeatureConfigurator.Configure();
        ServerDemoModuleExtensionConfigurator.Configure();
        
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<ServerDemoDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new ServerDemoDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}