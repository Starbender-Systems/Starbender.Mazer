using Volo.Abp.AspNetCore.Components.WebAssembly.Theming.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Starbender.AbpMudTheme.WebAssembly.Bundling;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingBundlingModule)
)]
public class AbpMudThemeWebAssemblyBundlingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            var globalStyles = options.StyleBundles.Get(BlazorWebAssemblyStandardBundles.Styles.Global);
            globalStyles.AddContributors(typeof(AbpMudThemeBundleStyleContributor));

            var globalScripts = options.ScriptBundles.Get(BlazorWebAssemblyStandardBundles.Scripts.Global);
            globalScripts.AddContributors(typeof(AbpMudThemeBundleScriptContributor));
        });
    }
}
