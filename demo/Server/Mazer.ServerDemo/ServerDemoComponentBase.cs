using Mazer.ServerDemo.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Mazer.ServerDemo;

public abstract class ServerDemoComponentBase : AbpComponentBase
{
    protected ServerDemoComponentBase()
    {
        LocalizationResource = typeof(ServerDemoResource);
    }
}
