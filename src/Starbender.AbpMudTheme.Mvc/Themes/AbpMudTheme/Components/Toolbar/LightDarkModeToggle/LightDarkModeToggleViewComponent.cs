using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Starbender.AbpMudTheme.Mvc.Themes.AbpMudTheme.Components.Toolbar.LightDarkModeToggle;

public class LightDarkModeToggleViewComponent : AbpViewComponent
{
    public virtual Task<IViewComponentResult> InvokeAsync()
    {
        return Task.FromResult<IViewComponentResult>(
            View("~/Themes/AbpMudTheme/Components/Toolbar/LightDarkModeToggle/Default.cshtml")
        );
    }
}
