using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Starbender.Mazer.Services;
using Volo.Abp.AspNetCore.Components.Messages;

namespace Starbender.Mazer.Components.Theme;

public partial class UiMessageAlert : ComponentBase, IDisposable
{
    private bool IsVisible { get; set; }

    private UiMessageType MessageType { get; set; }

    private string? Title { get; set; }

    private string Message { get; set; } = string.Empty;

    private UiMessageOptions? Options { get; set; }

    private TaskCompletionSource<bool>? Callback { get; set; }

    [Inject]
    protected MudBlazorUiMessageService UiMessageService { get; set; } = default!;

    private bool IsConfirmation => MessageType == UiMessageType.Confirmation;

    private bool ShowMessageIcon => Options?.ShowMessageIcon ?? true;

    private bool IsMessageHtmlMarkup => Options?.IsMessageHtmlMarkup ?? false;

    private string MessageIcon => Options?.MessageIcon as string ?? MessageType switch
    {
        UiMessageType.Info => Icons.Material.Filled.Info,
        UiMessageType.Success => Icons.Material.Filled.CheckCircle,
        UiMessageType.Warning => Icons.Material.Filled.Warning,
        UiMessageType.Error => Icons.Material.Filled.Error,
        UiMessageType.Confirmation => Icons.Material.Filled.Help,
        _ => Icons.Material.Filled.Info
    };

    private string OkButtonText => Options?.OkButtonText ?? "OK";

    private string ConfirmButtonText => Options?.ConfirmButtonText ?? "Confirm";

    private string CancelButtonText => Options?.CancelButtonText ?? "Cancel";

    protected override void OnInitialized()
    {
        base.OnInitialized();
        UiMessageService.MessageReceived += OnMessageReceived;
    }

    private async void OnMessageReceived(object? sender, UiMessageEventArgs e)
    {
        MessageType = e.MessageType;
        Message = e.Message;
        Title = e.Title;
        Options = e.Options;
        Callback = e.Callback;
        IsVisible = true;

        await InvokeAsync(StateHasChanged);
    }

    private static Color GetColor(UiMessageType type)
    {
        return type switch
        {
            UiMessageType.Info => Color.Info,
            UiMessageType.Success => Color.Success,
            UiMessageType.Warning => Color.Warning,
            UiMessageType.Error => Color.Error,
            UiMessageType.Confirmation => Color.Primary,
            _ => Color.Default
        };
    }

    private Task OnOkClicked()
    {
        IsVisible = false;
        return InvokeAsync(StateHasChanged);
    }

    private Task OnConfirmClicked()
    {
        IsVisible = false;
        Callback?.TrySetResult(true);
        return InvokeAsync(StateHasChanged);
    }

    private Task OnCancelClicked()
    {
        IsVisible = false;
        Callback?.TrySetResult(false);
        return InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        UiMessageService.MessageReceived -= OnMessageReceived;
    }
}
