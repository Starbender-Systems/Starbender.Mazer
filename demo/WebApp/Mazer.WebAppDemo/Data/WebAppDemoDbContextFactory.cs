using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mazer.WebAppDemo.Data;

public class WebAppDemoDbContextFactory : IDesignTimeDbContextFactory<WebAppDemoDbContext>
{
    public WebAppDemoDbContext CreateDbContext(string[] args)
    {
        WebAppDemoGlobalFeatureConfigurator.Configure();
        WebAppDemoModuleExtensionConfigurator.Configure();
        
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<WebAppDemoDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new WebAppDemoDbContext(builder.Options);
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