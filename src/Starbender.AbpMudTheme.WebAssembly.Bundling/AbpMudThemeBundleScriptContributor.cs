using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Starbender.AbpMudTheme.WebAssembly.Bundling;

public class AbpMudThemeBundleScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains(AbpMudThemeAssetFiles.MudBlazorJs);
        context.Files.AddIfNotContains(AbpMudThemeAssetFiles.ThemeModeJs);
    }
}
