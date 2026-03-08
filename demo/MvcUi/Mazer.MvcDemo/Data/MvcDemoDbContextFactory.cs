using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mazer.MvcDemo.Data;

public class MvcDemoDbContextFactory : IDesignTimeDbContextFactory<MvcDemoDbContext>
{
    public MvcDemoDbContext CreateDbContext(string[] args)
    {
        MvcDemoGlobalFeatureConfigurator.Configure();
        MvcDemoModuleExtensionConfigurator.Configure();

        MvcDemoEfCoreEntityExtensionMappings.Configure();
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<MvcDemoDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new MvcDemoDbContext(builder.Options);
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