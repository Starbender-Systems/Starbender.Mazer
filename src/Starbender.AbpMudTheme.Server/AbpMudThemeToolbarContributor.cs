using Starbender.AbpMudTheme.Components.Theme;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;

namespace Starbender.AbpMudTheme.Server;

public class AbpMudThemeToolbarContributor : IToolbarContributor
{
    public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name == StandardToolbars.Main)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(LightDarkModeToggle)));
            context.Toolbar.Items.Add(new ToolbarItem(typeof(LoginDisplay)));
            context.Toolbar.Items.Add(new ToolbarItem(typeof(LanguageSwitch)));
        }

        return Task.CompletedTask;
    }
}
