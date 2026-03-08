using Volo.Abp.Application.Services;
using Mazer.WebAssemblyDemo.Localization;

namespace Mazer.WebAssemblyDemo.Services;

/* Inherit your application services from this class. */
public abstract class WebAssemblyDemoAppService : ApplicationService
{
    protected WebAssemblyDemoAppService()
    {
        LocalizationResource = typeof(WebAssemblyDemoResource);
    }
}