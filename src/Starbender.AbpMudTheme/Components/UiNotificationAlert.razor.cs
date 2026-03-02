using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Starbender.AbpMudTheme.Services;
using Volo.Abp.AspNetCore.Components.Notifications;
using Volo.Abp.Localization;

namespace Starbender.AbpMudTheme.Components;

public partial class UiNotificationAlert : ComponentBase, IDisposable
{
    [Parameter] public EventCallback Okayed { get; set; }

    [Parameter] public EventCallback Closed { get; set; }

    [Inject]
    protected MudBlazorUiNotificationService UiNotificationService { get; set; } = default!;

    [Inject]
    protected IStringLocalizerFactory StringLocalizerFactory { get; set; } = default!;

    private List<NotificationItem> Notifications { get; } = new();

    protected override void OnInitialized()
    {
        base.OnInitialized();
        UiNotificationService.NotificationReceived += OnNotificationReceived;
    }

    private async void OnNotificationReceived(object? sender, UiNotificationEventArgs e)
    {
        var item = new NotificationItem
        {
            Id = Guid.NewGuid(),
            Type = e.NotificationType,
            Message = e.Message,
            Title = e.Title,
            OkButtonText = e.Options.OkButtonText == null
                ? null
                : await e.Options.OkButtonText.LocalizeAsync(StringLocalizerFactory)
        };

        Notifications.Add(item);
        await InvokeAsync(StateHasChanged);
        _ = AutoDismissAsync(item.Id, 10000);
    }

    private async Task AutoDismissAsync(Guid id, int milliseconds)
    {
        await Task.Delay(milliseconds);

        var removed = Notifications.RemoveAll(x => x.Id == id) > 0;
        if (removed)
        {
            await Closed.InvokeAsync();
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task DismissOk(Guid id)
    {
        Notifications.RemoveAll(x => x.Id == id);
        await Okayed.InvokeAsync();
        await InvokeAsync(StateHasChanged);
    }

    private async Task DismissClosed(Guid id)
    {
        Notifications.RemoveAll(x => x.Id == id);
        await Closed.InvokeAsync();
        await InvokeAsync(StateHasChanged);
    }

    private static Color GetColor(UiNotificationType notificationType)
    {
        return notificationType switch
        {
            UiNotificationType.Info => Color.Info,
            UiNotificationType.Success => Color.Success,
            UiNotificationType.Warning => Color.Warning,
            UiNotificationType.Error => Color.Error,
            _ => Color.Default
        };
    }

    public void Dispose()
    {
        UiNotificationService.NotificationReceived -= OnNotificationReceived;
    }

    private sealed class NotificationItem
    {
        public Guid Id { get; set; }

        public UiNotificationType Type { get; set; }

        public string Message { get; set; } = string.Empty;

        public string? Title { get; set; }

        public string? OkButtonText { get; set; }
    }
}
