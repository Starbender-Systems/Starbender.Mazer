using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace Starbender.AbpMudTheme.Mvc;

[ThemeName(Name)]
public class StarbenderTheme : ITheme, ITransientDependency
{
    public const string Name = "Starbender";

    public virtual string GetLayout(string name, bool fallbackToDefault = true)
    {
        switch (name)
        {
            case StandardLayouts.Application:
                return "~/Themes/AbpMudTheme/Layouts/Application.cshtml";
            case StandardLayouts.Account:
                return "~/Themes/AbpMudTheme/Layouts/Account.cshtml";
            case StandardLayouts.Empty:
                return "~/Themes/AbpMudTheme/Layouts/Empty.cshtml";
            default:
                return fallbackToDefault ? "~/Themes/AbpMudTheme/Layouts/Application.cshtml" : null;
        }
    }
}
