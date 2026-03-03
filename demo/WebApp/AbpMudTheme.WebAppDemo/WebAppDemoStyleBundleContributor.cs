using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace AbpMudTheme.WebAppDemo;

public class WebAppDemoStyleBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add(new BundleFile("main.css", true));
    }
}
