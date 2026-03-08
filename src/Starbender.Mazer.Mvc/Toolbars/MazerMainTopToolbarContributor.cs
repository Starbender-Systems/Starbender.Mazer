using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Starbender.Mazer.Mvc.Themes.Mazer.Components.Toolbar.LightDarkModeToggle;
using Starbender.Mazer.Mvc.Themes.Mazer.Components.Toolbar.LanguageSwitch;
using Starbender.Mazer.Mvc.Themes.Mazer.Components.Toolbar.UserMenu;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.Localization;
using Volo.Abp.Users;

namespace Starbender.Mazer.Mvc.Toolbars;

public class MazerMainTopToolbarContributor : IToolbarContributor
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
