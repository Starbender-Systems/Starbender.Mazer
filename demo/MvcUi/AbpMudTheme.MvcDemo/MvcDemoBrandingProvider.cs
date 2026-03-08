using Microsoft.Extensions.Localization;
using AbpMudTheme.MvcDemo.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace AbpMudTheme.MvcDemo;

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