using Microsoft.Extensions.Localization;
using AbpMudTheme.ServerDemo.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace AbpMudTheme.ServerDemo;

[Dependency(ReplaceServices = true)]
public class ServerDemoBrandingProvider : DefaultBrandingProvider
{
    private readonly IStringLocalizer<ServerDemoResource> _localizer;

    public ServerDemoBrandingProvider(IStringLocalizer<ServerDemoResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
