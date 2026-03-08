using Microsoft.Extensions.Localization;
using Mazer.WebAssemblyDemo.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Mazer.WebAssemblyDemo;

[Dependency(ReplaceServices = true)]
public class WebAssemblyDemoBrandingProvider : DefaultBrandingProvider
{
    private readonly IStringLocalizer<WebAssemblyDemoResource> _localizer;

    public WebAssemblyDemoBrandingProvider(IStringLocalizer<WebAssemblyDemoResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}