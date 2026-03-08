using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using Mazer.WebAppDemo.Localization;

namespace Mazer.WebAppDemo;

[Dependency(ReplaceServices = true)]
public class WebAppDemoBrandingProvider : DefaultBrandingProvider
{
    private readonly IStringLocalizer<WebAppDemoResource> _localizer;

    public WebAppDemoBrandingProvider(IStringLocalizer<WebAppDemoResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
