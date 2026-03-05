using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Starbender.AbpMudTheme.Components.Theme;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;

namespace Starbender.AbpMudTheme.WebAssembly;

public class AbpMudThemeToolbarContributor : IToolbarContributor
{
    public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name == StandardToolbars.Main)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(LightDarkModeToggle)));
            context.Toolbar.Items.Add(new ToolbarItem(typeof(LanguageSwitch)));

            //TODO: Can we find a different way to understand if authentication was configured or not?
            var authenticationStateProvider = context.ServiceProvider
                .GetService<AuthenticationStateProvider>();

            if (authenticationStateProvider != null)
            {
                context.Toolbar.Items.Add(new ToolbarItem(typeof(LoginDisplay)));
            }
        }

        return Task.CompletedTask;
    }
}
