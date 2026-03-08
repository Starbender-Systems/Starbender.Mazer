using MudBlazor;
using MudBlazor.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Starbender.AbpMudTheme.Utilities;

/// <summary>
/// Serializes and deserializes <see cref="MudTheme"/> values.
/// </summary>
public static class MudThemeJsonSerializer
{
    private static readonly MudThemeJsonSerializationContext _defaultContext = CreateContext(indented: false);
    private static readonly MudThemeJsonSerializationContext _indentedContext = CreateContext(indented: true);

    /// <summary>
    /// Serializes a <see cref="MudTheme"/> into JSON.
    /// </summary>
    /// <param name="mudTheme">The theme to serialize.</param>
    /// <param name="indented">When <c>true</c>, writes indented JSON.</param>
    /// <returns>The serialized JSON.</returns>
    public static string Serialize(MudTheme mudTheme, bool indented = false)
    {
        ArgumentNullException.ThrowIfNull(mudTheme);

        var json = indented
            ? JsonSerializer.Serialize(mudTheme, _indentedContext.MudTheme)
            : JsonSerializer.Serialize(mudTheme, _defaultContext.MudTheme);

        var jsonNode = JsonNode.Parse(json);
        RemoveTypeDiscriminators(jsonNode);
        return jsonNode?.ToJsonString(new JsonSerializerOptions { WriteIndented = indented }) ?? "{}";
    }

    /// <summary>
    /// Deserializes JSON into a <see cref="MudTheme"/>.
    /// </summary>
    /// <param name="json">The JSON to deserialize.</param>
    /// <returns>The deserialized theme.</returns>
    /// <exception cref="JsonException">Thrown when the JSON cannot be deserialized into a <see cref="MudTheme"/>.</exception>
    public static MudTheme Deserialize(string json)
    {
        ArgumentNullException.ThrowIfNull(json);

        var jsonNode = JsonNode.Parse(json);
        AddMissingTypeDiscriminators(jsonNode);
        NormalizeTypeDiscriminatorOrder(jsonNode);
        var normalizedJson = jsonNode?.ToJsonString() ?? "null";

        return JsonSerializer.Deserialize(normalizedJson, _defaultContext.MudTheme)
               ?? throw new JsonException("Unable to deserialize JSON into MudTheme.");
    }

    private static MudThemeJsonSerializationContext CreateContext(bool indented)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = indented
        };

        options.Converters.Add(new MudColorHexJsonConverter());

        return new MudThemeJsonSerializationContext(options);
    }

    private sealed class MudColorHexJsonConverter : JsonConverter<MudColor>
    {
        public override MudColor Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var colorValue = reader.GetString();
                ArgumentException.ThrowIfNullOrEmpty(colorValue);

                return new MudColor(colorValue);
            }

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                byte r = 0;
                byte g = 0;
                byte b = 0;
                var a = byte.MaxValue;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        return new MudColor(r, g, b, a);
                    }

                    if (reader.TokenType != JsonTokenType.PropertyName)
                    {
                        throw new JsonException("Invalid MudColor JSON payload.");
                    }

                    var propertyName = reader.GetString();
                    if (!reader.Read())
                    {
                        throw new JsonException("Invalid MudColor JSON payload.");
                    }

                    switch (propertyName)
                    {
                        case nameof(MudColor.R):
                            r = reader.GetByte();
                            break;
                        case nameof(MudColor.G):
                            g = reader.GetByte();
                            break;
                        case nameof(MudColor.B):
                            b = reader.GetByte();
                            break;
                        case nameof(MudColor.A):
                            a = reader.GetByte();
                            break;
                        default:
                            reader.Skip();
                            break;
                    }
                }
            }

            throw new JsonException("MudColor JSON value must be a string or object.");
        }

        public override void Write(Utf8JsonWriter writer, MudColor value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(value);

            var outputFormat = value.A == byte.MaxValue
                ? MudColorOutputFormats.Hex
                : MudColorOutputFormats.HexA;

            writer.WriteStringValue(value.ToString(outputFormat));
        }
    }

    private static void RemoveTypeDiscriminators(JsonNode? jsonNode)
    {
        switch (jsonNode)
        {
            case JsonObject jsonObject:
                jsonObject.Remove("$type");
                foreach (var property in jsonObject)
                {
                    RemoveTypeDiscriminators(property.Value);
                }

                break;
            case JsonArray jsonArray:
                foreach (var childNode in jsonArray)
                {
                    RemoveTypeDiscriminators(childNode);
                }

                break;
        }
    }

    private static void AddMissingTypeDiscriminators(JsonNode? jsonNode)
    {
        if (jsonNode is not JsonObject mudThemeObject)
        {
            return;
        }

        SetDiscriminatorIfMissing(mudThemeObject, nameof(MudTheme.PaletteLight), nameof(PaletteLight));
        SetDiscriminatorIfMissing(mudThemeObject, nameof(MudTheme.PaletteDark), nameof(PaletteDark));

        if (!TryGetObjectProperty(mudThemeObject, nameof(MudTheme.Typography), out var typographyObject))
        {
            return;
        }

        SetDiscriminatorIfMissing(typographyObject, nameof(Typography.Default), nameof(DefaultTypography));
        SetDiscriminatorIfMissing(typographyObject, nameof(Typography.H1), nameof(H1Typography));
        SetDiscriminatorIfMissing(typographyObject, nameof(Typography.H2), nameof(H2Typography));
        SetDiscriminatorIfMissing(typographyObject, nameof(Typography.H3), nameof(H3Typography));
        SetDiscriminatorIfMissing(typographyObject, nameof(Typography.H4), nameof(H4Typography));
        SetDiscriminatorIfMissing(typographyObject, nameof(Typography.H5), nameof(H5Typography));
        SetDiscriminatorIfMissing(typographyObject, nameof(Typography.H6), nameof(H6Typography));
        SetDiscriminatorIfMissing(typographyObject, nameof(Typography.Subtitle1), nameof(Subtitle1Typography));
        SetDiscriminatorIfMissing(typographyObject, nameof(Typography.Subtitle2), nameof(Subtitle2Typography));
        SetDiscriminatorIfMissing(typographyObject, nameof(Typography.Body1), nameof(Body1Typography));
        SetDiscriminatorIfMissing(typographyObject, nameof(Typography.Body2), nameof(Body2Typography));
        SetDiscriminatorIfMissing(typographyObject, nameof(Typography.Button), nameof(ButtonTypography));
        SetDiscriminatorIfMissing(typographyObject, nameof(Typography.Caption), nameof(CaptionTypography));
        SetDiscriminatorIfMissing(typographyObject, nameof(Typography.Overline), nameof(OverlineTypography));
    }

    private static void SetDiscriminatorIfMissing(JsonObject jsonObject, string propertyName, string discriminatorValue)
    {
        if (TryGetObjectProperty(jsonObject, propertyName, out var propertyObject) && propertyObject["$type"] is null)
        {
            propertyObject["$type"] = discriminatorValue;
        }
    }

    private static bool TryGetObjectProperty(JsonObject jsonObject, string propertyName, [NotNullWhen(true)] out JsonObject? value)
    {
        foreach (var property in jsonObject)
        {
            if (string.Equals(property.Key, propertyName, StringComparison.OrdinalIgnoreCase) && property.Value is JsonObject propertyObject)
            {
                value = propertyObject;
                return true;
            }
        }

        value = null;
        return false;
    }

    private static void NormalizeTypeDiscriminatorOrder(JsonNode? jsonNode)
    {
        switch (jsonNode)
        {
            case JsonObject jsonObject:
                if (jsonObject["$type"] is JsonNode typeValue)
                {
                    jsonObject.Remove("$type");
                    List<KeyValuePair<string, JsonNode?>> existingProperties = [];
                    foreach (var property in jsonObject)
                    {
                        existingProperties.Add(property);
                    }

                    jsonObject.Clear();
                    jsonObject["$type"] = typeValue;

                    foreach (var property in existingProperties)
                    {
                        jsonObject[property.Key] = property.Value;
                    }
                }

                foreach (var property in jsonObject)
                {
                    NormalizeTypeDiscriminatorOrder(property.Value);
                }

                break;
            case JsonArray jsonArray:
                foreach (var child in jsonArray)
                {
                    NormalizeTypeDiscriminatorOrder(child);
                }

                break;
        }
    }
}
