using Microsoft.Extensions.Localization;
using Mazer.ServerDemo.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Mazer.ServerDemo;

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
