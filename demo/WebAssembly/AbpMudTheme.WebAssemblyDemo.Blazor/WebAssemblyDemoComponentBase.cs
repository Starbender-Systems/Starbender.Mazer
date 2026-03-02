using AbpMudTheme.WebAssemblyDemo.Localization;
using Volo.Abp.AspNetCore.Components;

namespace AbpMudTheme.WebAssemblyDemo;

public abstract class WebAssemblyDemoComponentBase : AbpComponentBase
{
    protected WebAssemblyDemoComponentBase()
    {
        LocalizationResource = typeof(WebAssemblyDemoResource);
    }
}
