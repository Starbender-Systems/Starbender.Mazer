using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Starbender.AbpMudTheme.Mvc.Bundling;

public class AbpMudThemeGlobalStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add(new BundleFile("/" + AbpMudThemeAssetFiles.GoogleFontCss, true));
        context.Files.Add("/" + AbpMudThemeAssetFiles.MvcLayoutCss);
    }
}
