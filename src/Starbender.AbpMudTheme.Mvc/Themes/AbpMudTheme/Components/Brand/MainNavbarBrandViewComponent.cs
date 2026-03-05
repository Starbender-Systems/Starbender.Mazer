using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Starbender.AbpMudTheme.Mvc.Themes.AbpMudTheme.Components.Brand;

public class MainNavbarBrandViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Themes/AbpMudTheme/Components/Brand/Default.cshtml");
    }
}
