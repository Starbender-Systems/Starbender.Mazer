using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using Starbender.AbpMudTheme.Services;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.Theming;

namespace Starbender.AbpMudTheme;

public partial class MainLayout : IDisposable
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    protected MudBlazorThemeManager ThemeManager { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private bool IsDrawerOpen { get; set; } = true;
    private bool IsThemeReady { get; set; }

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += OnLocationChanged;
        ThemeManager.ThemeChanged += OnThemeChanged;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        try
        {
            var prefersDarkMode = await JSRuntime.InvokeAsync<bool>("abpMudBlazorTheme.getPreferredDarkMode");
            ThemeManager.SetDarkMode(prefersDarkMode);
            await SetBootstrapThemeModeAsync();
        }
        catch (InvalidOperationException)
        {
            // JS interop may be unavailable during prerendering.
        }
        catch (JSException)
        {
            // Keep default theme mode if the helper script is unavailable.
        }
        finally
        {
            IsThemeReady = true;
            await InvokeAsync(StateHasChanged);
        }
    }

    private void ToggleDrawer()
    {
        IsDrawerOpen = !IsDrawerOpen;
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
        ThemeManager.ThemeChanged -= OnThemeChanged;
    }

    private void OnLocationChanged(object sender, LocationChangedEventArgs e)
    {
        InvokeAsync(StateHasChanged);
    }

    private void OnThemeChanged()
    {
        InvokeAsync(async () =>
        {
            await SetBootstrapThemeModeAsync();
            StateHasChanged();
        });
    }

    private async Task SetBootstrapThemeModeAsync()
    {
        try
        {
            await JSRuntime.InvokeVoidAsync("abpMudBlazorTheme.setDarkMode", ThemeManager.IsDarkMode);
        }
        catch (InvalidOperationException)
        {
            // JS interop may be unavailable during prerendering.
        }
        catch (JSException)
        {
            // Ignore bootstrap theme sync failures and keep rendering.
        }
    }
}
