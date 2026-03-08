using System;
using System.Globalization;
using System.Text;
using MudBlazor;

namespace Starbender.Mazer.Mvc.Themes.Mazer.Styling;

public static class MudThemeCssVariablesBuilder
{
    public static string Build(MudTheme mudTheme)
    {
        var light = mudTheme?.PaletteLight;
        var dark = mudTheme?.PaletteDark;

        var lightPrimary = ResolveColor(light?.Primary, "#1D4ED8");
        var lightSecondary = ResolveColor(light?.Secondary, "#0EA5E9");
        var lightTertiary = ResolveColor(light?.Tertiary, "#334155");
        var lightBackground = ResolveColor(light?.Background, "#F8FAFC");
        var lightSurface = ResolveColor(light?.Surface, "#FFFFFF");
        var lightAppbarBackground = ResolveColor(light?.AppbarBackground, "#1E3A8A");
        var lightAppbarText = ResolveColor(light?.AppbarText, "#F8FAFC");
        var lightDrawerBackground = ResolveColor(light?.DrawerBackground, "#FFFFFF");
        var lightDrawerText = ResolveColor(light?.DrawerText, "#0F172A");
        var lightDrawerIcon = ResolveColor(light?.DrawerIcon, "#334155");
        var lightTextPrimary = ResolveColor(light?.TextPrimary, "#0F172A");
        var lightTextSecondary = ResolveColor(light?.TextSecondary, "#334155");
        var lightSuccess = ResolveColor(light?.Success, "#15803D");
        var lightWarning = ResolveColor(light?.Warning, "#B45309");
        var lightDanger = ResolveColor(light?.Error, "#B91C1C");
        var lightInfo = ResolveColor(light?.Info, "#0369A1");
        var lightActionDisabledBackground = ResolveColor(light?.ActionDisabledBackground, "#E2E8F0");
        var lightDivider = ResolveColor(light?.Divider, "#CBD5E1");
        var lightAppbarHover = ToRgba(lightAppbarText, 0.16m, "rgba(248, 250, 252, 0.16)");

        var darkPrimary = ResolveColor(dark?.Primary, "#60A5FA");
        var darkSecondary = ResolveColor(dark?.Secondary, "#38BDF8");
        var darkTertiary = ResolveColor(dark?.Tertiary, "#CBD5E1");
        var darkBackground = ResolveColor(dark?.Background, "#0B1220");
        var darkSurface = ResolveColor(dark?.Surface, "#111827");
        var darkAppbarBackground = ResolveColor(dark?.AppbarBackground, "#0F172A");
        var darkAppbarText = ResolveColor(dark?.AppbarText, "#E2E8F0");
        var darkDrawerBackground = ResolveColor(dark?.DrawerBackground, "#0F172A");
        var darkDrawerText = ResolveColor(dark?.DrawerText, "#E2E8F0");
        var darkDrawerIcon = ResolveColor(dark?.DrawerIcon, "#93C5FD");
        var darkTextPrimary = ResolveColor(dark?.TextPrimary, "#E5E7EB");
        var darkTextSecondary = ResolveColor(dark?.TextSecondary, "#CBD5E1");
        var darkSuccess = ResolveColor(dark?.Success, "#4ADE80");
        var darkWarning = ResolveColor(dark?.Warning, "#FBBF24");
        var darkDanger = ResolveColor(dark?.Error, "#F87171");
        var darkInfo = ResolveColor(dark?.Info, "#7DD3FC");
        var darkActionDisabledBackground = ResolveColor(dark?.ActionDisabledBackground, "#1F2937");
        var darkDivider = ResolveColor(dark?.Divider, "#334155");
        var darkAppbarHover = ToRgba(darkAppbarText, 0.14m, "rgba(226, 232, 240, 0.14)");

        var sb = new StringBuilder();

        sb.AppendLine(":root,");
        sb.AppendLine("[data-bs-theme=\"light\"] {");
        AppendVar(sb, "--abp-mud-primary", lightPrimary);
        AppendVar(sb, "--abp-mud-secondary", lightSecondary);
        AppendVar(sb, "--abp-mud-tertiary", lightTertiary);
        AppendVar(sb, "--abp-mud-background", lightBackground);
        AppendVar(sb, "--abp-mud-surface", lightSurface);
        AppendVar(sb, "--abp-mud-appbar-bg", lightAppbarBackground);
        AppendVar(sb, "--abp-mud-appbar-text", lightAppbarText);
        AppendVar(sb, "--abp-mud-drawer-bg", lightDrawerBackground);
        AppendVar(sb, "--abp-mud-drawer-text", lightDrawerText);
        AppendVar(sb, "--abp-mud-drawer-icon", lightDrawerIcon);
        AppendVar(sb, "--abp-mud-text-primary", lightTextPrimary);
        AppendVar(sb, "--abp-mud-text-secondary", lightTextSecondary);
        AppendVar(sb, "--abp-mud-success", lightSuccess);
        AppendVar(sb, "--abp-mud-warning", lightWarning);
        AppendVar(sb, "--abp-mud-danger", lightDanger);
        AppendVar(sb, "--abp-mud-info", lightInfo);
        AppendVar(sb, "--abp-mud-action-disabled-bg", lightActionDisabledBackground);
        AppendVar(sb, "--abp-mud-divider", lightDivider);
        AppendVar(sb, "--abp-mud-appbar-hover-bg", lightAppbarHover);

        AppendVar(sb, "--bs-primary", lightPrimary);
        AppendVar(sb, "--bs-primary-rgb", ToRgb(lightPrimary, "29, 78, 216"));
        AppendVar(sb, "--bs-secondary", lightSecondary);
        AppendVar(sb, "--bs-secondary-rgb", ToRgb(lightSecondary, "14, 165, 233"));
        AppendVar(sb, "--bs-success", lightSuccess);
        AppendVar(sb, "--bs-success-rgb", ToRgb(lightSuccess, "21, 128, 61"));
        AppendVar(sb, "--bs-warning", lightWarning);
        AppendVar(sb, "--bs-warning-rgb", ToRgb(lightWarning, "180, 83, 9"));
        AppendVar(sb, "--bs-danger", lightDanger);
        AppendVar(sb, "--bs-danger-rgb", ToRgb(lightDanger, "185, 28, 28"));
        AppendVar(sb, "--bs-info", lightInfo);
        AppendVar(sb, "--bs-info-rgb", ToRgb(lightInfo, "3, 105, 161"));
        AppendVar(sb, "--bs-body-bg", lightBackground);
        AppendVar(sb, "--bs-body-color", lightTextPrimary);
        AppendVar(sb, "--bs-border-color", lightDivider);
        AppendVar(sb, "--bs-tertiary-bg", lightActionDisabledBackground);
        AppendVar(sb, "--bs-dark", lightAppbarBackground);
        AppendVar(sb, "--bs-dark-rgb", ToRgb(lightAppbarBackground, "30, 58, 138"));
        sb.AppendLine("}");

        sb.AppendLine();
        sb.AppendLine("[data-bs-theme=\"dark\"] {");
        AppendVar(sb, "--abp-mud-primary", darkPrimary);
        AppendVar(sb, "--abp-mud-secondary", darkSecondary);
        AppendVar(sb, "--abp-mud-tertiary", darkTertiary);
        AppendVar(sb, "--abp-mud-background", darkBackground);
        AppendVar(sb, "--abp-mud-surface", darkSurface);
        AppendVar(sb, "--abp-mud-appbar-bg", darkAppbarBackground);
        AppendVar(sb, "--abp-mud-appbar-text", darkAppbarText);
        AppendVar(sb, "--abp-mud-drawer-bg", darkDrawerBackground);
        AppendVar(sb, "--abp-mud-drawer-text", darkDrawerText);
        AppendVar(sb, "--abp-mud-drawer-icon", darkDrawerIcon);
        AppendVar(sb, "--abp-mud-text-primary", darkTextPrimary);
        AppendVar(sb, "--abp-mud-text-secondary", darkTextSecondary);
        AppendVar(sb, "--abp-mud-success", darkSuccess);
        AppendVar(sb, "--abp-mud-warning", darkWarning);
        AppendVar(sb, "--abp-mud-danger", darkDanger);
        AppendVar(sb, "--abp-mud-info", darkInfo);
        AppendVar(sb, "--abp-mud-action-disabled-bg", darkActionDisabledBackground);
        AppendVar(sb, "--abp-mud-divider", darkDivider);
        AppendVar(sb, "--abp-mud-appbar-hover-bg", darkAppbarHover);

        AppendVar(sb, "--bs-primary", darkPrimary);
        AppendVar(sb, "--bs-primary-rgb", ToRgb(darkPrimary, "96, 165, 250"));
        AppendVar(sb, "--bs-secondary", darkSecondary);
        AppendVar(sb, "--bs-secondary-rgb", ToRgb(darkSecondary, "56, 189, 248"));
        AppendVar(sb, "--bs-success", darkSuccess);
        AppendVar(sb, "--bs-success-rgb", ToRgb(darkSuccess, "74, 222, 128"));
        AppendVar(sb, "--bs-warning", darkWarning);
        AppendVar(sb, "--bs-warning-rgb", ToRgb(darkWarning, "251, 191, 36"));
        AppendVar(sb, "--bs-danger", darkDanger);
        AppendVar(sb, "--bs-danger-rgb", ToRgb(darkDanger, "248, 113, 113"));
        AppendVar(sb, "--bs-info", darkInfo);
        AppendVar(sb, "--bs-info-rgb", ToRgb(darkInfo, "125, 211, 252"));
        AppendVar(sb, "--bs-body-bg", darkBackground);
        AppendVar(sb, "--bs-body-color", darkTextPrimary);
        AppendVar(sb, "--bs-border-color", darkDivider);
        AppendVar(sb, "--bs-tertiary-bg", darkActionDisabledBackground);
        AppendVar(sb, "--bs-dark", darkAppbarBackground);
        AppendVar(sb, "--bs-dark-rgb", ToRgb(darkAppbarBackground, "15, 23, 42"));
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string ResolveColor(object color, string fallback)
    {
        if (color == null)
        {
            return fallback;
        }

        if (color is string colorText)
        {
            return string.IsNullOrWhiteSpace(colorText) ? fallback : colorText;
        }

        if (TryReadRgbFromObject(color, out var r, out var g, out var b))
        {
            return $"#{r:X2}{g:X2}{b:X2}";
        }

        var value = color.ToString();
        return string.IsNullOrWhiteSpace(value) ? fallback : value;
    }

    private static void AppendVar(StringBuilder sb, string name, string value)
    {
        sb.Append("    ");
        sb.Append(name);
        sb.Append(": ");
        sb.Append(value);
        sb.AppendLine(";");
    }

    private static string ToRgb(string colorValue, string fallback)
    {
        if (TryParseRgb(colorValue, out var r, out var g, out var b))
        {
            return $"{r}, {g}, {b}";
        }

        return fallback;
    }

    private static string ToRgba(string colorValue, decimal alpha, string fallback)
    {
        if (TryParseRgb(colorValue, out var r, out var g, out var b))
        {
            return $"rgba({r}, {g}, {b}, {alpha.ToString(CultureInfo.InvariantCulture)})";
        }

        return fallback;
    }

    private static bool TryParseRgb(string value, out int r, out int g, out int b)
    {
        r = g = b = 0;
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var trimmed = value.Trim();
        if (trimmed.StartsWith("#", StringComparison.Ordinal))
        {
            return TryParseHex(trimmed, out r, out g, out b);
        }

        if (trimmed.StartsWith("rgb", StringComparison.OrdinalIgnoreCase))
        {
            return TryParseRgbFunction(trimmed, out r, out g, out b);
        }

        return false;
    }

    private static bool TryParseHex(string value, out int r, out int g, out int b)
    {
        r = g = b = 0;
        var hex = value[1..];

        if (hex.Length == 3 || hex.Length == 4)
        {
            if (!TryParseHexChannel(hex[0], hex[0], out r) ||
                !TryParseHexChannel(hex[1], hex[1], out g) ||
                !TryParseHexChannel(hex[2], hex[2], out b))
            {
                return false;
            }

            return true;
        }

        if (hex.Length == 6 || hex.Length == 8)
        {
            if (!TryParseHexChannel(hex[0], hex[1], out r) ||
                !TryParseHexChannel(hex[2], hex[3], out g) ||
                !TryParseHexChannel(hex[4], hex[5], out b))
            {
                return false;
            }

            return true;
        }

        return false;
    }

    private static bool TryParseHexChannel(char high, char low, out int value)
    {
        value = 0;
        var segment = string.Concat(high, low);
        return int.TryParse(segment, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
    }

    private static bool TryParseRgbFunction(string value, out int r, out int g, out int b)
    {
        r = g = b = 0;

        var openIndex = value.IndexOf('(');
        var closeIndex = value.LastIndexOf(')');
        if (openIndex < 0 || closeIndex <= openIndex)
        {
            return false;
        }

        var content = value[(openIndex + 1)..closeIndex];
        var parts = content.Split(',');
        if (parts.Length < 3)
        {
            return false;
        }

        return TryParseRgbChannel(parts[0], out r) &&
               TryParseRgbChannel(parts[1], out g) &&
               TryParseRgbChannel(parts[2], out b);
    }

    private static bool TryParseRgbChannel(string input, out int value)
    {
        value = 0;
        var channel = input.Trim();
        if (channel.EndsWith("%", StringComparison.Ordinal))
        {
            var percentValue = channel[..^1];
            if (!decimal.TryParse(percentValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var percent))
            {
                return false;
            }

            value = (int)Math.Clamp(Math.Round(percent * 2.55m), 0, 255);
            return true;
        }

        if (!decimal.TryParse(channel, NumberStyles.Float, CultureInfo.InvariantCulture, out var number))
        {
            return false;
        }

        value = (int)Math.Clamp(Math.Round(number), 0, 255);
        return true;
    }

    private static bool TryReadRgbFromObject(object color, out int r, out int g, out int b)
    {
        r = g = b = 0;
        var colorType = color.GetType();
        var redProperty = colorType.GetProperty("R");
        var greenProperty = colorType.GetProperty("G");
        var blueProperty = colorType.GetProperty("B");

        if (redProperty == null || greenProperty == null || blueProperty == null)
        {
            return false;
        }

        return TryConvertToColorChannel(redProperty.GetValue(color), out r) &&
               TryConvertToColorChannel(greenProperty.GetValue(color), out g) &&
               TryConvertToColorChannel(blueProperty.GetValue(color), out b);
    }

    private static bool TryConvertToColorChannel(object channelValue, out int channel)
    {
        channel = 0;
        if (channelValue == null)
        {
            return false;
        }

        if (channelValue is byte byteValue)
        {
            channel = byteValue;
            return true;
        }

        if (channelValue is int intValue)
        {
            channel = (int)Math.Clamp(intValue, 0, 255);
            return true;
        }

        var text = channelValue.ToString();
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        if (!int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
        {
            return false;
        }

        channel = (int)Math.Clamp(parsedValue, 0, 255);
        return true;
    }
}
