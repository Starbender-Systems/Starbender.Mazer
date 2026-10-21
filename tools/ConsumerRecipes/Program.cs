using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MudBlazor;
using MudBlazor.Utilities;

namespace Mazer.ConsumerExamples;

internal static class Program
{
    private static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            throw new ArgumentException("Expected inventory output path and extracted theme settings path.");
        }

        var settings = File.ReadAllText(args[1]);
        using var configuration = new ConfigurationManager();
        configuration.AddJsonFile(Path.GetFullPath(args[1]));
        CheckTheme(configuration);

        // Exercise the extension's alternate scalar-JSON input using the same recipe data.
        using var document = JsonDocument.Parse(settings);
        using var scalarConfiguration = new ConfigurationManager();
        scalarConfiguration.AddInMemoryCollection(
            new Dictionary<string, string?> { ["MudTheme"] = document.RootElement.GetProperty("MudTheme").GetRawText() });
        CheckTheme(scalarConfiguration);

        // Ensure the Razor files are compiled as components, not just copied as content.
        Type[] components = [typeof(Routes), typeof(AppServer), typeof(AppAuto), typeof(FeedbackPreview)];
        if (components.Any(type => !typeof(Microsoft.AspNetCore.Components.ComponentBase).IsAssignableFrom(type)))
        {
            throw new InvalidOperationException("A documented Razor example was not compiled as a component.");
        }

        Assembly[] assemblies =
        [
            typeof(Starbender.Mazer.MazerModule).Assembly,
            typeof(Starbender.Mazer.AbpMudBlazorThemeInstallerModule).Assembly,
            typeof(Starbender.Mazer.Mvc.MazerMvcModule).Assembly,
            typeof(Starbender.Mazer.Server.MazerServerModule).Assembly,
            typeof(Starbender.Mazer.MazerSharedModule).Assembly,
            typeof(Starbender.Mazer.WebAssembly.MazerWebAssemblyModule).Assembly,
            typeof(Starbender.Mazer.WebAssembly.Bundling.MazerWebAssemblyBundlingModule).Assembly
        ];

        // These are exported type IDs, not an exhaustive method/member compatibility report.
        var inventory = new
        {
            schema_version = 1,
            packages = assemblies.OrderBy(assembly => assembly.GetName().Name, StringComparer.Ordinal).Select(assembly => new
            {
                id = assembly.GetName().Name!,
                version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion.Split('+')[0],
                symbols = assembly.GetExportedTypes().Select(type => "T:" + type.FullName!.Replace('+', '.'))
                    .Order(StringComparer.Ordinal).ToArray()
            }).ToArray()
        };
        File.WriteAllText(args[0], JsonSerializer.Serialize(inventory, new JsonSerializerOptions { WriteIndented = true }) + "\n");
        Console.WriteLine($"Checked both configuration inputs and serializer round trips; inventoried {assemblies.Length} source assemblies.");
    }

    private static void CheckTheme(IConfiguration configuration)
    {
        using var services = ThemeConfiguration.AddBrandTheme(new ServiceCollection(), configuration).BuildServiceProvider();
        var configured = services.GetRequiredService<IOptions<MudTheme>>().Value;
        var restored = ThemeConfiguration.Import(ThemeConfiguration.Export(configured));
        foreach (var theme in new[] { configured, restored })
        {
            if (theme.PaletteLight.Primary != new MudColor("#435ebe") ||
                theme.PaletteDark.Primary != new MudColor("#8093e8") ||
                theme.LayoutProperties.DefaultBorderRadius != "6px")
            {
                throw new InvalidOperationException("The documented palette/layout configuration did not survive registration or serialization.");
            }
        }
    }
}
