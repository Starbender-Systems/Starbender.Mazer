using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Starbender.AbpMudTheme.Mvc.Themes.AbpMudTheme.Components.MainNavbar;

public class MainNavbarViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Themes/AbpMudTheme/Components/MainNavbar/Default.cshtml");
    }
}
