using Volo.Abp.Application.Services;
using AbpMudTheme.WebAssemblyDemo.Localization;

namespace AbpMudTheme.WebAssemblyDemo.Services;

/* Inherit your application services from this class. */
public abstract class WebAssemblyDemoAppService : ApplicationService
{
    protected WebAssemblyDemoAppService()
    {
        LocalizationResource = typeof(WebAssemblyDemoResource);
    }
}