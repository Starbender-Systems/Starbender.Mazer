using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Starbender.Mazer.Mvc.Themes.Mazer.Components.Toolbar.LightDarkModeToggle;

public class LightDarkModeToggleViewComponent : AbpViewComponent
{
    public virtual Task<IViewComponentResult> InvokeAsync()
    {
        return Task.FromResult<IViewComponentResult>(
            View("~/Themes/Mazer/Components/Toolbar/LightDarkModeToggle/Default.cshtml")
        );
    }
}
