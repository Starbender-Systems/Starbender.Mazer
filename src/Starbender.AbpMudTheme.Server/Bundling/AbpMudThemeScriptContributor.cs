using Starbender.AbpMudTheme.Shared;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Starbender.AbpMudTheme.Server.Bundling;

public class AbpMudThemeScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/" + AbpMudThemeAssetFiles.MudBlazorJs);
        context.Files.AddIfNotContains("/" + AbpMudThemeAssetFiles.ThemeModeJs);
    }
}
