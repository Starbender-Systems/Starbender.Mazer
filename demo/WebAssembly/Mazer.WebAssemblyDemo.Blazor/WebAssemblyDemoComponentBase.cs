using Mazer.WebAssemblyDemo.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Mazer.WebAssemblyDemo;

public abstract class WebAssemblyDemoComponentBase : AbpComponentBase
{
    protected WebAssemblyDemoComponentBase()
    {
        LocalizationResource = typeof(WebAssemblyDemoResource);
    }
}
