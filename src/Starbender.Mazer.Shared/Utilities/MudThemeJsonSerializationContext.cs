using System.Text.Json.Serialization;
using MudBlazor;

namespace Starbender.Mazer.Utilities;

[JsonSerializable(typeof(MudTheme))]
internal sealed partial class MudThemeJsonSerializationContext : JsonSerializerContext;

