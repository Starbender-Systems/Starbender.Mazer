using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;

namespace Starbender.Mazer.Mvc.Themes.Mazer.Components.Toolbar;

public class MainNavbarToolbarViewComponent : AbpViewComponent
{
    protected IToolbarManager ToolbarManager { get; }

    public MainNavbarToolbarViewComponent(IToolbarManager toolbarManager)
    {
        ToolbarManager = toolbarManager;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var toolbar = await ToolbarManager.GetAsync(StandardToolbars.Main);
        return View("~/Themes/Mazer/Components/Toolbar/Default.cshtml", toolbar);
    }
}
