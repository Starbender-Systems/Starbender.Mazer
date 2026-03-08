using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Starbender.Mazer.Server.Bundling;

public class MazerScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/" + MazerAssetFiles.MudBlazorJs);
        context.Files.AddIfNotContains("/" + MazerAssetFiles.ThemeModeJs);
    }
}
