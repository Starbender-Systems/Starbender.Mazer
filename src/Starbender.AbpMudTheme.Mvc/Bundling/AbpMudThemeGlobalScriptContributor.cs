using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Starbender.AbpMudTheme.Mvc.Bundling;

public class AbpMudThemeGlobalScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add("/" + AbpMudThemeAssetFiles.MvcLayoutJs);
    }
}
