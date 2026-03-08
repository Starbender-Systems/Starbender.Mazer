using Volo.Abp.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Mazer.WebAssemblyDemo.Data;

public class WebAssemblyDemoDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public WebAssemblyDemoDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        
        /* We intentionally resolving the WebAssemblyDemoDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<WebAssemblyDemoDbContext>()
            .Database
            .MigrateAsync();

    }
}
