using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Mazer.WebAssemblyDemo;

public class WebAssemblyDemoStyleBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add(new BundleFile("main.css", true));
    }
}