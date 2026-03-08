using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using Volo.Abp.UI.Navigation;

namespace Starbender.Mazer.Server;

public partial class LoginDisplay : IDisposable
{
    [Inject]
    protected IMenuManager MenuManager { get; set; }

    protected ApplicationMenu Menu { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Menu = await MenuManager.GetAsync(StandardMenus.User);

        Navigation.LocationChanged += OnLocationChanged;
    }

    protected virtual void OnLocationChanged(object sender, LocationChangedEventArgs e)
    {
        InvokeAsync(StateHasChanged);
    }

    protected virtual async Task NavigateToAsync(string uri, string target = null)
    {
        var normalizedUri = NormalizeMenuUri(uri);

        if (target == "_blank")
        {
            await JsRuntime.InvokeVoidAsync("open", normalizedUri, target);
            return;
        }

        Navigation.NavigateTo(normalizedUri, forceLoad: true);
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

    public void Dispose()
    {
        Navigation.LocationChanged -= OnLocationChanged;
    }
}
