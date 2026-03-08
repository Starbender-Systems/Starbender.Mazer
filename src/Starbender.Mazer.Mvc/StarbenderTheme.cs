using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace Starbender.Mazer.Mvc;

[ThemeName(Name)]
public class StarbenderTheme : ITheme, ITransientDependency
{
    public const string Name = "Starbender";

    public virtual string GetLayout(string name, bool fallbackToDefault = true)
    {
        switch (name)
        {
            case StandardLayouts.Application:
                return "~/Themes/Mazer/Layouts/Application.cshtml";
            case StandardLayouts.Account:
                return "~/Themes/Mazer/Layouts/Account.cshtml";
            case StandardLayouts.Empty:
                return "~/Themes/Mazer/Layouts/Empty.cshtml";
            default:
                return fallbackToDefault ? "~/Themes/Mazer/Layouts/Application.cshtml" : null;
        }
    }
}
