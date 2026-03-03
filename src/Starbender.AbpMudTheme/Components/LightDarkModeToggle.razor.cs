using Volo.Abp.AspNetCore.Components;

namespace Starbender.AbpMudTheme.Components;

public partial class LightDarkModeToggle : AbpComponentBase
{
    private void ToggleDarkMode()
    {
        ThemeManager.ToggleDarkMode();
    }
}
