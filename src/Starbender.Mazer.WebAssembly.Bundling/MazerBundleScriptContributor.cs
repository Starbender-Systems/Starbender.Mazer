using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Starbender.Mazer.WebAssembly.Bundling;

public class MazerBundleScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains(MazerAssetFiles.MudBlazorJs);
        context.Files.AddIfNotContains(MazerAssetFiles.ThemeModeJs);
        context.Files.AddIfNotContains(MazerAssetFiles.MvcLayoutJs);
    }
}
