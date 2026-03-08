using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Starbender.Mazer.Server.Bundling;

public class MazerStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/" + MazerAssetFiles.MudBlazorCss);
        context.Files.AddIfNotContains("/" + MazerAssetFiles.ThemeCss);
    }
}
