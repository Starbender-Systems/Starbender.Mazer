using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Starbender.AbpMudTheme.Mvc.Themes.AbpMudTheme.Components.Toolbar.LightDarkModeToggle;
using Starbender.AbpMudTheme.Mvc.Themes.AbpMudTheme.Components.Toolbar.LanguageSwitch;
using Starbender.AbpMudTheme.Mvc.Themes.AbpMudTheme.Components.Toolbar.UserMenu;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.Localization;
using Volo.Abp.Users;

namespace Starbender.AbpMudTheme.Mvc.Toolbars;

public class AbpMudThemeMainTopToolbarContributor : IToolbarContributor
{
    public async Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name != StandardToolbars.Main)
        {
            return;
        }

        if (context.Theme is not StarbenderTheme)
        {
            return;
        }

        context.Toolbar.Items.Add(new ToolbarItem(typeof(LightDarkModeToggleViewComponent)));

        var languageProvider = context.ServiceProvider.GetService<ILanguageProvider>();

        //TODO: This duplicates GetLanguages() usage. Can we eleminate this?
        var languages = await languageProvider.GetLanguagesAsync();
        if (languages.Count > 1)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(LanguageSwitchViewComponent)));
        }

        if (context.ServiceProvider.GetRequiredService<ICurrentUser>().IsAuthenticated)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(UserMenuViewComponent)));
        }
    }
}
