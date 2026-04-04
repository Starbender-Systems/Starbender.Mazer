using Volo.Abp.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Mazer.WebAppDemo.Data;

public class WebAppDemoDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public WebAppDemoDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {

        /* We intentionally resolving the WebAppDemoDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<WebAppDemoDbContext>()
            .Database
            .MigrateAsync();

    }
}
