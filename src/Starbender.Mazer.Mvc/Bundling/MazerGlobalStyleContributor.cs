using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Starbender.Mazer.Mvc.Bundling;

public class MazerGlobalStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add(new BundleFile("/" + MazerAssetFiles.GoogleFontCss, true));
        context.Files.Add("/" + MazerAssetFiles.MvcLayoutCss);
    }
}
