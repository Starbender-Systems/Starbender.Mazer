using Starbender.Mazer.Server.Bundling;
using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.AspNetCore.Components.Server.Theming.Bundling;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Starbender.Mazer.Server;

[DependsOn(
    typeof(MazerModule),
    typeof(AbpAspNetCoreComponentsServerThemingModule)
    )]
public class MazerServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new MazerToolbarContributor());
        });

        Configure<AbpBundlingOptions>(options =>
        {
            var globalStyles = options.StyleBundles.Get(BlazorStandardBundles.Styles.Global);
            globalStyles.AddContributors(typeof(MazerStyleContributor));

            var globalScripts = options.ScriptBundles.Get(BlazorStandardBundles.Scripts.Global);
            globalScripts.AddContributors(typeof(MazerScriptContributor));
        });
    }
}
