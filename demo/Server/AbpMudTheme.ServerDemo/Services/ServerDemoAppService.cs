using Volo.Abp.Application.Services;
using AbpMudTheme.ServerDemo.Localization;

namespace AbpMudTheme.ServerDemo.Services;

/* Inherit your application services from this class. */
public abstract class ServerDemoAppService : ApplicationService
{
    protected ServerDemoAppService()
    {
        LocalizationResource = typeof(ServerDemoResource);
    }
}