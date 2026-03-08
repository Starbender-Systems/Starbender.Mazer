using Starbender.Mazer.Components.Theme;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;

namespace Starbender.Mazer.Server;

public class MazerToolbarContributor : IToolbarContributor
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
