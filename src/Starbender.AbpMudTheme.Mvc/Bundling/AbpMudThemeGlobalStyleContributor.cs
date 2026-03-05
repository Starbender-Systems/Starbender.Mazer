using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Starbender.AbpMudTheme.Mvc.Bundling;

public class AbpMudThemeGlobalStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add(new BundleFile("/themes/abpmudtheme/googlefonts.css", true));
        context.Files.Add("/themes/abpmudtheme/layout.css");
    }
}
