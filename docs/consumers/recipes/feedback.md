# Show Blazor messages, notifications, and progress

`Starbender.Mazer.Blazor` supplies scoped replacements for ABP's `IUiMessageService`, `IUiNotificationService`, and `IUiPageProgressService`. Use them from interactive Blazor components after the [Mazer layout](blazor-layout.md) is active.

<!-- recipe-check: FeedbackPreview.razor -->
```razor
@namespace Mazer.ConsumerExamples
@using Volo.Abp.AspNetCore.Components.Messages
@using Volo.Abp.AspNetCore.Components.Notifications
@using Volo.Abp.AspNetCore.Components.Progression
@inject IUiMessageService Messages
@inject IUiNotificationService Notifications
@inject IUiPageProgressService Progress

<button @onclick="PreviewAsync">Preview feedback</button>

@code {
    private async Task PreviewAsync()
    {
        if (!await Messages.Confirm("Show a sample notification?", "UI preview"))
        {
            return;
        }

        await Progress.Go(null);
        try
        {
            await Notifications.Success("The feedback services are connected.");
        }
        finally
        {
            await Progress.Go(-1);
        }
    }
}
```

`MainLayout` contains the matching `UiMessageAlert`, `UiNotificationAlert`, and `UiPageProgress` listeners. Preserve those once in a custom layout, along with the MudBlazor providers. Without a message listener, `Confirm` has no UI to complete its returned task. Notification/progress events also need mounted listeners. Preserve the concrete service registrations used by the listeners when adapting DI; replacing only an interface can disconnect the publisher from the component.

The message service has `Info`, `Success`, `Warn`, `Error`, and `Confirm`. Confirmation completes when the user responds; ordinary message methods return after raising the event. Default button labels are localized through ABP UI resources. Leave `IsMessageHtmlMarkup` disabled for untrusted text.

The notification bridge raises events consumed by Mazer's `UiNotificationAlert`, which dismisses notifications after ten seconds and supports its close/OK callbacks. It does not forward ABP notifications to `ISnackbar`. `MazerModule` separately registers MudBlazor services with snackbar defaults, and `MainLayout` includes a snackbar provider for components that explicitly use that API.

Progress accepts `null` for indeterminate display and 0 through 100 for determinate display. The current component hides on an out-of-range value such as `-1`; use cleanup in `finally`. These services are Blazor UI services; MVC pages use their existing ABP UI integration.

Acceptance: approve and cancel a confirmation, show/dismiss a notification, verify the normal and exception progress cleanup paths, and navigate away/back to check listener disposal. Compile checks do not exercise interactive event delivery or browser rendering.
