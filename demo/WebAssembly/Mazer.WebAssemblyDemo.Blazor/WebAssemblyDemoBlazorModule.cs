using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Components.WebAssembly.Theming.Bundling;
using Mazer.WebAssemblyDemo.Menus;
using OpenIddict.Abstractions;
using Volo.Abp.Account;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.OpenIddict;
using Volo.Abp.Identity.Blazor.WebAssembly;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.Blazor.WebAssembly;
using Volo.Abp.Autofac.WebAssembly;
using Volo.Abp.Mapperly;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.Blazor.WebAssembly;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.Blazor.WebAssembly;
using Volo.Abp.SettingManagement.Blazor.WebAssembly;
using Volo.Abp.SettingManagement;
using Volo.Abp.UI.Navigation;
using Starbender.Mazer.WebAssembly;
using MudBlazor.Services;
using MudBlazor;
using Starbender.Mazer.Extensions;

namespace Mazer.WebAssemblyDemo;

[DependsOn(
    typeof(WebAssemblyDemoContractsModule),

    // ABP Framework packages
    typeof(AbpAutofacWebAssemblyModule),

    // Account module packages
    typeof(AbpAccountHttpApiClientModule),

    // OpenIddict module packages
    typeof(AbpOpenIddictDomainSharedModule),

    // Identity module packages
    typeof(AbpIdentityBlazorWebAssemblyModule),
    typeof(AbpIdentityHttpApiClientModule),

    // Language Management module packages
    typeof(AbpTenantManagementBlazorWebAssemblyModule),
    typeof(AbpTenantManagementHttpApiClientModule),

    // Permission Management module packages
    typeof(AbpPermissionManagementBlazorWebAssemblyModule),
    typeof(AbpPermissionManagementHttpApiClientModule),

    // Feature Management module packages
    typeof(AbpFeatureManagementBlazorWebAssemblyModule),
    typeof(AbpFeatureManagementHttpApiClientModule),

    // Setting Management module packages
    typeof(AbpSettingManagementHttpApiClientModule),
    typeof(AbpSettingManagementBlazorWebAssemblyModule),

    // Theme
    typeof(MazerWebAssemblyModule)
)]
public class WebAssemblyDemoBlazorModule : AbpModule
{
    public const string RemoteServiceName = "Default";

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpAspNetCoreComponentsWebOptions>(options =>
        {
            options.IsBlazorWebApp = true;
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var environment = context.Services.GetSingletonInstance<IWebAssemblyHostEnvironment>();
        var builder = context.Services.GetSingletonInstance<WebAssemblyHostBuilder>();

        ConfigureAuthentication(builder);
        ConfigureHttpClient(context, environment);
        ConfigureHttpClientProxies(context);
        ConfigureBlazorise(context);
        ConfigureMudBlazor(context);
        ConfigureRouter(context);
        ConfigureMenu(context);
    }

    private void ConfigureRouter(ServiceConfigurationContext _)
    {
        Configure<AbpRouterOptions>(options =>
        {
            options.AppAssembly = typeof(WebAssemblyDemoBlazorModule).Assembly;
        });
    }

    private void ConfigureMenu(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new WebAssemblyDemoMenuContributor(context.Services.GetConfiguration()));
        });
    }

    private void ConfigureBlazorise(ServiceConfigurationContext context)
    {
        context.Services
            .AddBootstrap5Providers()
            .AddFontAwesomeIcons();
    }

    private void ConfigureMudBlazor(ServiceConfigurationContext context)
    {
        // The MudBlazor Services are already configured but you may 
        // override the defaults here. If you don't want any additional 
        // customizations, you can remove the AddMudServices(...) here
        //context.Services.AddMudServices(config =>
        //{
        //    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
        //    config.SnackbarConfiguration.RequireInteraction = false;
        //    config.SnackbarConfiguration.PreventDuplicates = false;
        //    config.SnackbarConfiguration.NewestOnTop = false;
        //    config.SnackbarConfiguration.ShowCloseIcon = true;
        //    config.SnackbarConfiguration.VisibleStateDuration = 5000;
        //    config.SnackbarConfiguration.HideTransitionDuration = 500;
        //    config.SnackbarConfiguration.ShowTransitionDuration = 500;
        //    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        //});

        // You can override the Mazer here
        //Configure<MudTheme>(theme =>
        //{
        //    theme.LayoutProperties.DrawerWidthLeft = "200px";
        //});

        // You can load a theme from the configuration
        context.Services.AddMudTheme(context.Configuration.GetSection("MudTheme"));
    }

    private static void ConfigureAuthentication(WebAssemblyHostBuilder builder)
    {
        builder.Services.AddBlazorWebAppServices();
    }

    private static void ConfigureHttpClient(ServiceConfigurationContext context, IWebAssemblyHostEnvironment environment)
    {
        context.Services.AddTransient(sp => new HttpClient
        {
            BaseAddress = new Uri(environment.BaseAddress)
        });
    }

    private void ConfigureHttpClientProxies(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(WebAssemblyDemoContractsModule).Assembly,
            RemoteServiceName
        );
    }
}
