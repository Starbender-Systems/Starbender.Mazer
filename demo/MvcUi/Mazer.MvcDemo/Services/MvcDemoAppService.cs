using Volo.Abp.Application.Services;
using Mazer.MvcDemo.Localization;

namespace Mazer.MvcDemo.Services;

/* Inherit your application services from this class. */
public abstract class MvcDemoAppService : ApplicationService
{
    protected MvcDemoAppService()
    {
        LocalizationResource = typeof(MvcDemoResource);
    }
}