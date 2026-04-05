using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using Volo.Abp.AspNetCore.Components.Web.Security;
using Volo.Abp.UI.Navigation;

namespace Starbender.Mazer.WebAssembly.Components;

public partial class LoginDisplay : IDisposable
{
    [Inject]
    protected IMenuManager MenuManager { get; set; }

    [Inject]
    protected ApplicationConfigurationChangedService ApplicationConfigurationChangedService { get; set; }

    protected ApplicationMenu Menu { get; set; }

    protected async override Task OnInitializedAsync()
    {
        Menu = await MenuManager.GetAsync(StandardMenus.User);

        Navigation.LocationChanged += OnLocationChanged;

        ApplicationConfigurationChangedService.Changed += ApplicationConfigurationChanged;
    }

    protected virtual void OnLocationChanged(object sender, LocationChangedEventArgs e)
    {
        InvokeAsync(StateHasChanged);
    }

    private async void ApplicationConfigurationChanged()
    {
        Menu = await MenuManager.GetAsync(StandardMenus.User);
        await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        Navigation.LocationChanged -= OnLocationChanged;
        ApplicationConfigurationChangedService.Changed -= ApplicationConfigurationChanged;
    }

    private async Task NavigateToAsync(string uri, string target = null)
    {
        var normalizedUri = NormalizeMenuUri(uri);

        if (target == "_blank")
        {
            await JsRuntime.InvokeVoidAsync("open", normalizedUri, target);
        }
        else
        {
            Navigation.NavigateTo(normalizedUri, forceLoad: true);
        }
    }

    private static string NormalizeMenuUri([NotNullIfNotNull(nameof(uri))] string uri)
    {
        if (string.IsNullOrWhiteSpace(uri))
        {
            return "/";
        }

        return uri.StartsWith("~/", StringComparison.Ordinal)
            ? uri[1..]
            : uri;
    }

    private void BeginSignOut()
    {
        if (AbpAspNetCoreComponentsWebOptions.Value.IsBlazorWebApp)
        {
            Navigation.NavigateTo(AuthenticationOptions.Value.LogoutUrl, forceLoad: true);
        }
        else
        {
            Navigation.NavigateToLogout(AuthenticationOptions.Value.LogoutUrl);
        }
    }
}

