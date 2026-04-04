using Volo.Abp.AspNetCore.Components;

namespace Starbender.Mazer.Components.Theme;

public partial class LightDarkModeToggle : AbpComponentBase
{
    private void ToggleDarkMode()
    {
        ThemeManager.ToggleDarkMode();
    }
}
