using Microsoft.Extensions.Localization;
using Mazer.MvcDemo.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Mazer.MvcDemo;

[Dependency(ReplaceServices = true)]
public class MvcDemoBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<MvcDemoResource> _localizer;

    public MvcDemoBrandingProvider(IStringLocalizer<MvcDemoResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}