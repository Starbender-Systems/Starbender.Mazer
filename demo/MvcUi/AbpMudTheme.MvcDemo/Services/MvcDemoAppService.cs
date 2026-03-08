using Volo.Abp.Application.Services;
using AbpMudTheme.MvcDemo.Localization;

namespace AbpMudTheme.MvcDemo.Services;

/* Inherit your application services from this class. */
public abstract class MvcDemoAppService : ApplicationService
{
    protected MvcDemoAppService()
    {
        LocalizationResource = typeof(MvcDemoResource);
    }
}