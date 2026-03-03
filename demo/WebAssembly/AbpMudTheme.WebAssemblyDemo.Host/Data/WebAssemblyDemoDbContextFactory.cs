using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AbpMudTheme.WebAssemblyDemo.Data;

public class WebAssemblyDemoDbContextFactory : IDesignTimeDbContextFactory<WebAssemblyDemoDbContext>
{
    public WebAssemblyDemoDbContext CreateDbContext(string[] args)
    {
        WebAssemblyDemoGlobalFeatureConfigurator.Configure();
        WebAssemblyDemoModuleExtensionConfigurator.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<WebAssemblyDemoDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new WebAssemblyDemoDbContext(builder.Options);
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
