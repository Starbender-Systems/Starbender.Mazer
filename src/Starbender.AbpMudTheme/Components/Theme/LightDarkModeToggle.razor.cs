using Volo.Abp.AspNetCore.Components;

namespace Starbender.AbpMudTheme.Components.Theme;

public partial class LightDarkModeToggle : AbpComponentBase
{
    private void ToggleDarkMode()
    {
        ThemeManager.ToggleDarkMode();
    }
}
