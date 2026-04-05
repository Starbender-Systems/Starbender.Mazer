using Volo.Abp.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Mazer.MvcDemo.Data;

public class MvcDemoDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public MvcDemoDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {

        /* We intentionally resolving the MvcDemoDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<MvcDemoDbContext>()
            .Database
            .MigrateAsync();

    }
}
