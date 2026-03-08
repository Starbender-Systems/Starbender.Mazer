using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Starbender.Mazer.WebAssembly.Bundling;

public class MazerBundleStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains(MazerAssetFiles.MudBlazorCss);
        context.Files.AddIfNotContains(MazerAssetFiles.ThemeCss);
        context.Files.AddIfNotContains(MazerAssetFiles.MvcLayoutCss);
        context.Files.AddIfNotContains(new BundleFile(MazerAssetFiles.GoogleFontCss, true));
    }
}
