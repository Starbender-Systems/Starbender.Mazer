using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using Starbender.AbpMudTheme.Utilities;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Nodes;

namespace Starbender.AbpMudTheme.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Deserializes a configuration section into <see cref="MudTheme"/> options.
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="configurationSection">The configuration section containing <see cref="MudTheme"/> values.</param>
    /// <returns>Continues the IServiceCollection chain.</returns>
    public static IServiceCollection AddMudTheme(this IServiceCollection services, IConfigurationSection configurationSection)
    {
        ArgumentNullException.ThrowIfNull(configurationSection);

        var mudTheme = MudThemeJsonSerializer.Deserialize(GetMudThemeJson(configurationSection));

        services.AddOptions<MudTheme>()
            .Configure(options =>
            {
                options.PaletteLight = mudTheme.PaletteLight;
                options.PaletteDark = mudTheme.PaletteDark;
                options.Shadows = mudTheme.Shadows;
                options.Typography = mudTheme.Typography;
                options.LayoutProperties = mudTheme.LayoutProperties;
                options.ZIndex = mudTheme.ZIndex;
                options.PseudoCss = mudTheme.PseudoCss;
            });

        return services;
    }

    private static string GetMudThemeJson(IConfigurationSection configurationSection)
    {
        if (!string.IsNullOrWhiteSpace(configurationSection.Value))
        {
            return configurationSection.Value!;
        }

        return ConvertConfigurationSectionToJsonNode(configurationSection)?.ToJsonString() ?? "{}";
    }

    private static JsonNode? ConvertConfigurationSectionToJsonNode(IConfigurationSection configurationSection)
    {
        IReadOnlyList<IConfigurationSection> children = [.. configurationSection.GetChildren()];
        if (children.Count == 0)
        {
            return ConvertScalarToJsonNode(configurationSection.Value);
        }

        if (TryConvertChildrenToArray(children, out var jsonArray))
        {
            return jsonArray;
        }

        var jsonObject = new JsonObject();
        foreach (var child in children)
        {
            jsonObject[child.Key] = ConvertConfigurationSectionToJsonNode(child);
        }

        return jsonObject;
    }

    private static bool TryConvertChildrenToArray(IReadOnlyList<IConfigurationSection> children, [NotNullWhen(true)] out JsonArray? jsonArray)
    {
        jsonArray = null;
        var indexedChildren = new Dictionary<int, IConfigurationSection>();
        var maxIndex = -1;

        foreach (var child in children)
        {
            if (!int.TryParse(child.Key, NumberStyles.None, CultureInfo.InvariantCulture, out var index) || index < 0 || indexedChildren.ContainsKey(index))
            {
                return false;
            }

            indexedChildren[index] = child;
            if (index > maxIndex)
            {
                maxIndex = index;
            }
        }

        if (maxIndex != children.Count - 1)
        {
            return false;
        }

        jsonArray = [];
        for (var index = 0; index <= maxIndex; index++)
        {
            jsonArray.Add(ConvertConfigurationSectionToJsonNode(indexedChildren[index]));
        }

        return true;
    }

    private static JsonNode? ConvertScalarToJsonNode(string? value)
    {
        if (value is null)
        {
            return null;
        }

        if (bool.TryParse(value, out var boolValue))
        {
            return JsonValue.Create(boolValue);
        }

        if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
        {
            return JsonValue.Create(intValue);
        }

        if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longValue))
        {
            return JsonValue.Create(longValue);
        }

        if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var doubleValue))
        {
            return JsonValue.Create(doubleValue);
        }

        return JsonValue.Create(value);
    }
}
