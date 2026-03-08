using Volo.Abp.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Mazer.ServerDemo.Data;

public class ServerDemoDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public ServerDemoDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        
        /* We intentionally resolving the ServerDemoDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<ServerDemoDbContext>()
            .Database
            .MigrateAsync();

    }
}
