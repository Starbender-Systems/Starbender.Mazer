using Volo.Abp.Application.Services;
using AbpMudTheme.WebAppDemo.Localization;

namespace AbpMudTheme.WebAppDemo.Services;

/* Inherit your application services from this class. */
public abstract class WebAppDemoAppService : ApplicationService
{
    protected WebAppDemoAppService()
    {
        LocalizationResource = typeof(WebAppDemoResource);
    }
}