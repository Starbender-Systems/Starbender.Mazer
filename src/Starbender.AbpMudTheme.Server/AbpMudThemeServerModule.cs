using Starbender.AbpMudTheme.Server.Bundling;
using Starbender.AbpMudTheme;
using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.AspNetCore.Components.Server.Theming.Bundling;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Starbender.AbpMudTheme.Server;

[DependsOn(
    typeof(AbpMudThemeModule),
    typeof(AbpAspNetCoreComponentsServerThemingModule)
    )]
public class AbpMudThemeServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new AbpMudThemeToolbarContributor());
        });

        Configure<AbpBundlingOptions>(options =>
        {
            var globalStyles = options.StyleBundles.Get(BlazorStandardBundles.Styles.Global);
            globalStyles.AddContributors(typeof(AbpMudThemeStyleContributor));

            var globalScripts = options.ScriptBundles.Get(BlazorStandardBundles.Scripts.Global);
            globalScripts.AddContributors(typeof(AbpMudThemeScriptContributor));
        });
    }
}
