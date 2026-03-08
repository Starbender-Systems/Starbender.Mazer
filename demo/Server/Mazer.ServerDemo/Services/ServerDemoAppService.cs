using Volo.Abp.Application.Services;
using Mazer.ServerDemo.Localization;

namespace Mazer.ServerDemo.Services;

/* Inherit your application services from this class. */
public abstract class ServerDemoAppService : ApplicationService
{
    protected ServerDemoAppService()
    {
        LocalizationResource = typeof(ServerDemoResource);
    }
}