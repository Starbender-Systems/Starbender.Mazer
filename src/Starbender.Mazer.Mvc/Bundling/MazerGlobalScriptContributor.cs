using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Starbender.Mazer.Mvc.Bundling;

public class MazerGlobalScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add("/" + MazerAssetFiles.MvcLayoutJs);
    }
}
