using Volo.Abp.AspNetCore.Components.WebAssembly.Theming.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Starbender.Mazer.WebAssembly.Bundling;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingBundlingModule)
)]
public class MazerWebAssemblyBundlingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            var globalStyles = options.StyleBundles.Get(BlazorMazerWebAssemblyBundles.Styles.Global);
            globalStyles.AddContributors(typeof(MazerBundleStyleContributor));

            var globalScripts = options.ScriptBundles.Get(BlazorMazerWebAssemblyBundles.Scripts.Global);
            globalScripts.AddContributors(typeof(MazerBundleScriptContributor));
        });
    }
}
