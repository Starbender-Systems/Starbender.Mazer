using Microsoft.Extensions.Options;
using MudBlazor;
using System;
using Volo.Abp.DependencyInjection;

namespace Starbender.AbpMudTheme.Services;

public class MudBlazorThemeManager : IScopedDependency, IDisposable
{
    private readonly IOptionsMonitor<MudTheme> _themeMonitor;
    private readonly IDisposable _subscription;
    private MudTheme _theme = new();

    public MudBlazorThemeManager(IOptionsMonitor<MudTheme> themeMonitor)
    {
        _themeMonitor = themeMonitor;
        _subscription = _themeMonitor.OnChange(SetTheme);
        _theme = themeMonitor.CurrentValue;
    }

    public event Action? ThemeChanged;

    public bool IsDarkMode { get; private set; }

    public MudTheme Theme => _theme;

    public void ToggleDarkMode()
    {
        IsDarkMode = !IsDarkMode;
        ThemeChanged?.Invoke();
    }

    public void SetTheme(MudTheme newTheme)
    {
        if (Theme == newTheme)
        {
            return;
        }

        _theme = newTheme;
        ThemeChanged?.Invoke();
    }

    public void SetDarkMode(bool isDarkMode)
    {
        if (IsDarkMode == isDarkMode)
        {
            return;
        }

        IsDarkMode = isDarkMode;
        ThemeChanged?.Invoke();
    }

    public void Dispose()
    {
        _subscription.Dispose();
    }
}
