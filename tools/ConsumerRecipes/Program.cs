using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MudBlazor;
using MudBlazor.Utilities;
using Volo.Abp.Modularity;
using BlazorThemingOptions = Volo.Abp.AspNetCore.Components.Web.Theming.Theming.AbpThemingOptions;
using MvcThemingOptions = Volo.Abp.AspNetCore.Mvc.UI.Theming.AbpThemingOptions;

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
        CheckThemeDefaultsAndInvalidJson();
        CheckThemeSelection();

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
        Console.WriteLine($"Checked configuration inputs, defaults, invalid JSON, serialization, and theme selection; inventoried {assemblies.Length} source assemblies.");
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

    private static void CheckThemeDefaultsAndInvalidJson()
    {
        foreach (var sectionValue in new string?[] { null, "{}" })
        {
            using var configuration = new ConfigurationManager();
            if (sectionValue is not null)
            {
                configuration.AddInMemoryCollection(new Dictionary<string, string?> { ["MudTheme"] = sectionValue });
            }

            using var services = ThemeConfiguration.AddBrandTheme(new ServiceCollection(), configuration).BuildServiceProvider();
            var actual = services.GetRequiredService<IOptions<MudTheme>>().Value;
            if (ThemeConfiguration.Export(actual) != ThemeConfiguration.Export(new MudTheme()))
            {
                throw new InvalidOperationException("Missing/empty theme settings did not produce default MudTheme values.");
            }
        }

        foreach (var invalidJson in new[] { "null", "{invalid-json" })
        {
            using var configuration = new ConfigurationManager();
            configuration.AddInMemoryCollection(new Dictionary<string, string?> { ["MudTheme"] = invalidJson });
            try
            {
                ThemeConfiguration.AddBrandTheme(new ServiceCollection(), configuration);
            }
            catch (JsonException)
            {
                continue;
            }

            throw new InvalidOperationException("Invalid theme JSON was accepted unexpectedly.");
        }
    }

    private static void CheckThemeSelection()
    {
        foreach (var oldTheme in new[] { "Basic", "LeptonX", "LeptonXLite" })
        {
            var services = new ServiceCollection();
            services.Configure<MvcThemingOptions>(options => options.DefaultThemeName = oldTheme);
            services.Configure<BlazorThemingOptions>(options => options.DefaultThemeName = oldTheme);
            var context = new ServiceConfigurationContext(services);
            new MazerMvcThemeSelectionModule().ConfigureServices(context);
            new MazerBlazorThemeSelectionModule().ConfigureServices(context);
            // Even ordinary configuration registered later runs before PostConfigure.
            services.Configure<MvcThemingOptions>(options => options.DefaultThemeName = oldTheme);
            services.Configure<BlazorThemingOptions>(options => options.DefaultThemeName = oldTheme);
            using var provider = services.BuildServiceProvider();
            if (provider.GetRequiredService<IOptions<MvcThemingOptions>>().Value.DefaultThemeName != Starbender.Mazer.Mvc.StarbenderTheme.Name ||
                provider.GetRequiredService<IOptions<BlazorThemingOptions>>().Value.DefaultThemeName != Starbender.Mazer.StarbenderTheme.Name)
            {
                throw new InvalidOperationException($"Documented theme selection did not replace the {oldTheme} default.");
            }
        }
    }
}
