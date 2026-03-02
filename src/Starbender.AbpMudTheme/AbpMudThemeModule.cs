using MudBlazor;
using MudBlazor.Services;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AspNetCore.Components.Web.Theming.Theming;
using Volo.Abp.Modularity;

namespace Starbender.AbpMudTheme;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingModule)
)]
public class AbpMudThemeModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpThemingOptions>(options =>
        {
            options.Themes.Add<MudBlazorTheme>();

            if (options.DefaultThemeName == null)
            {
                options.DefaultThemeName = MudBlazorTheme.Name;
            }
        });

        ConfigureMudBlazor(context);
    }

    private void ConfigureMudBlazor(ServiceConfigurationContext context)
    {
        context.Services.AddMudServices();

        Configure<MudTheme>(cfg =>
        {
            cfg.PaletteLight = new PaletteLight
            {
                Primary = "#1D4ED8",           // blue-700
                Secondary = "#0EA5E9",         // sky-500
                Tertiary = "#334155",          // slate-700

                Background = "#F8FAFC",        // slate-50
                Surface = "#FFFFFF",

                AppbarBackground = "#1E3A8A",  // blue-900
                AppbarText = "#F8FAFC",

                DrawerBackground = "#FFFFFF",
                DrawerText = "#0F172A",        // slate-900
                DrawerIcon = "#334155",

                TextPrimary = "#0F172A",
                TextSecondary = "#334155",

                Success = "#15803D",           // green-700
                Warning = "#B45309",           // amber-700
                Error = "#B91C1C",             // red-700
                Info = "#0369A1",              // sky-700

                ActionDefault = "#1E40AF",
                ActionDisabled = "#94A3B8",
                ActionDisabledBackground = "#E2E8F0",

                Divider = "#CBD5E1",
                LinesDefault = "#E2E8F0"
            };

            cfg.PaletteDark = new PaletteDark
            {
                Primary = "#60A5FA",           // blue-400
                Secondary = "#38BDF8",         // sky-400
                Tertiary = "#CBD5E1",          // slate-300

                Background = "#0B1220",
                Surface = "#111827",           // gray-900

                AppbarBackground = "#0F172A",  // slate-900
                AppbarText = "#E2E8F0",

                DrawerBackground = "#0F172A",
                DrawerText = "#E2E8F0",
                DrawerIcon = "#93C5FD",

                TextPrimary = "#E5E7EB",
                TextSecondary = "#CBD5E1",

                Success = "#4ADE80",           // green-400
                Warning = "#FBBF24",           // amber-400
                Error = "#F87171",             // red-400
                Info = "#7DD3FC",              // sky-300

                ActionDefault = "#93C5FD",
                ActionDisabled = "#64748B",
                ActionDisabledBackground = "#1F2937",

                Divider = "#334155",
                LinesDefault = "#334155"
            };

            cfg.LayoutProperties = new LayoutProperties
            {
                DrawerWidthLeft = "300px"
            };
        });
    }
}
