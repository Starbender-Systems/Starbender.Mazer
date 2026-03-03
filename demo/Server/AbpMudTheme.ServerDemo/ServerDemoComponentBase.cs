using AbpMudTheme.ServerDemo.Localization;
using Volo.Abp.AspNetCore.Components;

namespace AbpMudTheme.ServerDemo;

public abstract class ServerDemoComponentBase : AbpComponentBase
{
    protected ServerDemoComponentBase()
    {
        LocalizationResource = typeof(ServerDemoResource);
    }
}
