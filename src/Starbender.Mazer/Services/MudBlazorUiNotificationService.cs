using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Notifications;
using Volo.Abp.DependencyInjection;

namespace Starbender.Mazer.Services;

[Dependency(ReplaceServices = true)]
public class MudBlazorUiNotificationService : IUiNotificationService, IScopedDependency
{
    public event EventHandler<UiNotificationEventArgs>? NotificationReceived;

    public Task Info(string message, string? title = null, Action<UiNotificationOptions>? options = null)
    {
        var uiNotificationOptions = CreateDefaultOptions();
        options?.Invoke(uiNotificationOptions);

        NotificationReceived?.Invoke(this, new UiNotificationEventArgs(UiNotificationType.Info, message, title, uiNotificationOptions));
        return Task.CompletedTask;
    }

    public Task Success(string message, string? title = null, Action<UiNotificationOptions>? options = null)
    {
        var uiNotificationOptions = CreateDefaultOptions();
        options?.Invoke(uiNotificationOptions);

        NotificationReceived?.Invoke(this, new UiNotificationEventArgs(UiNotificationType.Success, message, title, uiNotificationOptions));
        return Task.CompletedTask;
    }

    public Task Warn(string message, string? title = null, Action<UiNotificationOptions>? options = null)
    {
        var uiNotificationOptions = CreateDefaultOptions();
        options?.Invoke(uiNotificationOptions);

        NotificationReceived?.Invoke(this, new UiNotificationEventArgs(UiNotificationType.Warning, message, title, uiNotificationOptions));
        return Task.CompletedTask;
    }

    public Task Error(string message, string? title = null, Action<UiNotificationOptions>? options = null)
    {
        var uiNotificationOptions = CreateDefaultOptions();
        options?.Invoke(uiNotificationOptions);

        NotificationReceived?.Invoke(this, new UiNotificationEventArgs(UiNotificationType.Error, message, title, uiNotificationOptions));
        return Task.CompletedTask;
    }

    protected virtual UiNotificationOptions CreateDefaultOptions()
    {
        return new UiNotificationOptions();
    }
}
