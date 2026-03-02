using AbpMudTheme.WebAppDemo.Localization;
using Volo.Abp.AspNetCore.Components;

namespace AbpMudTheme.WebAppDemo;

public abstract class WebAppDemoComponentBase : AbpComponentBase
{
    protected WebAppDemoComponentBase()
    {
        LocalizationResource = typeof(WebAppDemoResource);
    }
}
