using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Alerts;

namespace Starbender.AbpMudTheme.Mvc.Themes.AbpMudTheme.Components.PageAlerts;

public class PageAlertsViewComponent : AbpViewComponent
{
    protected IAlertManager AlertManager { get; }

    public PageAlertsViewComponent(IAlertManager alertManager)
    {
        AlertManager = alertManager;
    }

    public IViewComponentResult Invoke(string _)
    {
        return View("~/Themes/AbpMudTheme/Components/PageAlerts/Default.cshtml", AlertManager.Alerts);
    }
}
