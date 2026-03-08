using Mazer.WebAppDemo.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Mazer.WebAppDemo;

public abstract class WebAppDemoComponentBase : AbpComponentBase
{
    protected WebAppDemoComponentBase()
    {
        LocalizationResource = typeof(WebAppDemoResource);
    }
}
