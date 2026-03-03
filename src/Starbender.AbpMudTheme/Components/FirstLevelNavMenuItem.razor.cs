using Microsoft.AspNetCore.Components;
using Volo.Abp.UI.Navigation;

namespace Starbender.AbpMudTheme.Components;

public partial class FirstLevelNavMenuItem
{
    [Parameter]
    public ApplicationMenuItem MenuItem { get; set; } = default!;

    protected virtual string GetMudIcon(string icon)
    {
        return string.IsNullOrWhiteSpace(icon) ? null : icon;
    }
}
