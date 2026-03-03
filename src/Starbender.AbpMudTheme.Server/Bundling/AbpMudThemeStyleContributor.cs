using System.Collections.Generic;

using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Starbender.AbpMudTheme.Server.Bundling;

public class AbpMudThemeStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/" + AbpMudThemeAssetFiles.MudBlazorCss);
        context.Files.AddIfNotContains("/" + AbpMudThemeAssetFiles.ThemeCss);
    }
}
