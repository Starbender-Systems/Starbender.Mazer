using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Alerts;

namespace Starbender.Mazer.Components.Theme;

public partial class PageAlert : ComponentBase, IDisposable
{
    private readonly List<PageAlertItem> _alerts = new();

    [Inject]
    protected IAlertManager AlertManager { get; set; } = default!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        NavigationManager.LocationChanged += NavigationManagerOnLocationChanged;
        AlertManager.Alerts.CollectionChanged += AlertsOnCollectionChanged;

        _alerts.AddRange(AlertManager.Alerts.Select(x => new PageAlertItem(x)));
    }

    private void NavigationManagerOnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        // Keep parity with existing ABP Blazorise behavior.
        AlertManager.Alerts.Clear();
        _alerts.Clear();
    }

    private void AlertsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
        {
            foreach (var item in e.NewItems.OfType<AlertMessage>())
            {
                _alerts.Add(new PageAlertItem(item));
            }
        }

        InvokeAsync(StateHasChanged);
    }

    private static Severity GetSeverity(AlertType alertType)
    {
        return alertType switch
        {
            AlertType.Info => Severity.Info,
            AlertType.Success => Severity.Success,
            AlertType.Warning => Severity.Warning,
            AlertType.Danger => Severity.Error,
            AlertType.Primary => Severity.Info,
            AlertType.Secondary => Severity.Normal,
            _ => Severity.Normal
        };
    }

    private void Dismiss(PageAlertItem alert)
    {
        alert.IsVisible = false;
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= NavigationManagerOnLocationChanged;
        AlertManager.Alerts.CollectionChanged -= AlertsOnCollectionChanged;
    }

    private sealed class PageAlertItem
    {
        public PageAlertItem(AlertMessage alertMessage)
        {
            AlertMessage = alertMessage;
        }

        public AlertMessage AlertMessage { get; }

        public bool IsVisible { get; set; } = true;
    }
}
