using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Security;
using Volo.Abp.UI.Navigation;

namespace Starbender.AbpMudTheme.Components.Theme;

public partial class NavMenu : IDisposable
{
    [Parameter] public bool Row { get; set; }

    [Inject]
    protected IMenuManager MenuManager { get; set; }

    [Inject]
    protected ApplicationConfigurationChangedService ApplicationConfigurationChangedService { get; set; }

    protected ApplicationMenu Menu { get; set; }

    protected async override Task OnInitializedAsync()
    {
        Menu = await MenuManager.GetMainMenuAsync();
        ApplicationConfigurationChangedService.Changed += ApplicationConfigurationChanged;
    }

    private async void ApplicationConfigurationChanged()
    {
        Menu = await MenuManager.GetMainMenuAsync();
        await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        ApplicationConfigurationChangedService.Changed -= ApplicationConfigurationChanged;
    }
}
