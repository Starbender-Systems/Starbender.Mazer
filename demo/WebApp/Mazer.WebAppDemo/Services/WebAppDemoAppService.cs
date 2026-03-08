using Volo.Abp.Application.Services;
using Mazer.WebAppDemo.Localization;

namespace Mazer.WebAppDemo.Services;

/* Inherit your application services from this class. */
public abstract class WebAppDemoAppService : ApplicationService
{
    protected WebAppDemoAppService()
    {
        LocalizationResource = typeof(WebAppDemoResource);
    }
}