using Starbender.AbpMudTheme.Shared;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Starbender.AbpMudTheme.WebAssembly.Bundling;

public class AbpMudThemeBundleStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains(AbpMudThemeAssetFiles.MudBlazorCss);
        context.Files.AddIfNotContains(AbpMudThemeAssetFiles.ThemeCss);
        context.Files.AddIfNotContains(new BundleFile(AbpMudThemeAssetFiles.GoogleFontCss, true));
        context.Files.AddIfNotContains(AbpMudThemeAssetFiles.MvcLayoutCss);
    }
}
