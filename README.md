# Starbender.Mazer
## ℹ️ Description
This is a [MudBlazor](https://mudblazor.com) based theme for Blazor applications that use the [ABP Framework](https://abp.io).

## Community

- Contribution guide: [`.github/CONTRIBUTING.md`](.github/CONTRIBUTING.md)
- Code of conduct: [`.github/CODE_OF_CONDUCT.md`](.github/CODE_OF_CONDUCT.md)
- Security policy: [`.github/SECURITY.md`](.github/SECURITY.md)

## Converting Existing Projects
Chances are that you have an existing ABP Blazor project with a different ABP theme. 
The process to convert to the Mazer is relatively straightforward for most applications.
The exact steps to take depend on:
1. You current theme provider
1. The Blazor model you are using (Server, WebAssembly or WebApp)
1. The amount of custom control development you have done.

If you are starting from scratch, the easiest thing to do is [create a "Basic Theme" app](https://abp.io/get-started) 
then follow the examples below for converting the Basic Theme boilerplate app of your chosen model.

### Convert a boilerplate Basic Theme application

#### Blazor Server
1. Install the Starbender.Mazer.Server nuget package in you Blazor Server application project. 
1. In your Blazor Server project module file (e.g. **MyBlazorApp**Module.cs) (usually in the root of the project, name ends in Module and always inherits from AbpModule)
   1. Update usings
        ```CS
        // Add 
        using MudBlazor;
        using MudBlazor.Services;
        using Starbender.Mazer.Server;
        
        // Remove 
        using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;                       // Keep (for now)
        using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;              // Keep (for now)
        // using Volo.Abp.AspNetCore.Components.Server.BasicTheme;          // Remove
        // using Volo.Abp.AspNetCore.Components.Server.BasicTheme.Bundling; // Remove
        ```
   1. Replace the Basic Theme module dependency in the DependsOn attribute of the module class.
	    ```CS
	    // Theme module packages
 	    typeof(AbpAspNetCoreMvcUiBasicThemeModule),                 // Leave this (for now)
 	    //typeof(AbpAspNetCoreComponentsServerBasicThemeModule),    // Remove this line
	    typeof(MazerServerModule),                            // Add the Mazer module
	    ```
   1. Add a placeholder method to do MudBlazor specific customizations
	    ```CS
        private void ConfigureMudBlazor(ServiceConfigurationContext context)
        {
            // You can override the Mazer here
            // Note: DO NOT try to assign a new MudTheme(), you must set properties on the 
            // instance passed to Configure
            Configure<MudTheme>(theme =>
            {
                theme.LayoutProperties.DrawerWidthLeft = "300px";
            });
            
            // The MudBlazor Services are already configured but you may 
            // override the defaults here. If you don't want any additional 
            // customizations, you can remove the AddMudServices(...) here
            context.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
                config.SnackbarConfiguration.RequireInteraction = false;
                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 5000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });
        }
	    ```
    1. Update the ConfigureBlazorise(...) method
	    ```CS
        private void ConfigureBlazorise(ServiceConfigurationContext context)
        {
            context.Services
            // If you are completely replacing the Basic Theme with Mazer,
            // you should remove "AddBlazorise(...)" below.
            // If you are doing a phased replacement you can leave it until you finish converting 
            // any code that is dependent on the Basic theme
            //.AddBlazorise(options =>
            //{
            //    options.ProductToken = "Your Product Token";
            //})
            .AddBootstrap5Providers()   // Leave this (for now)
            .AddFontAwesomeIcons();     // Leave this (for now)
        }
	    ```
    1. Remove the Basic Bundles 
	    ```CS
        private void ConfigureBundles()
        {
            Configure<AbpBundlingOptions>(options =>
            {
                // Leave this (for now)
                options.StyleBundles.Configure(
                    BasicThemeBundles.Styles.Global,
                    bundle =>
                    {
                        bundle.AddFiles("/global-styles.css");
                    }
                );

                // Leave this (for now)
                options.ScriptBundles.Configure(
                    BasicThemeBundles.Scripts.Global,
                    bundle =>
                    {
                        bundle.AddFiles("/global-scripts.js");
                    }
                );

                // Remove the BlazorBasicThemeBundles
                // options.StyleBundles.Configure(
                //    BlazorBasicThemeBundles.Styles.Global,
                //    bundle =>
                //    {
                //        bundle.AddFiles("/global-styles.css");
                //    }
                // );
            });
        }
	    ```
    1. Call ConfigureMudBlazor in ConfigureServices(...)
        ```CS
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // ...
            ConfigureBundles();             // DO NOT DELETE
            ConfigureBlazorise(context);    // Leave this (for now)
            ConfigureMudBlazor(context);    // Add this
            
            // ...
        }
	    ```
1. Update the reference to MainLayout.razor(often in Routes.razor)    
    ```cshtml
    // Add 
    @using Starbender.Mazer.Components
    @using Starbender.Mazer.Layouts
        
    // Remove 
    // @using Volo.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic
    ```
1. Update the theme bundles (often in App.razor)
    ```cshtml
    // Add 
    @using Starbender.Mazer.Components
    @using Starbender.Mazer.Layouts
        
    // Remove 
    // @using Volo.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic
    ```
1. If your application is boiler plate or you have no other Basic Theme customizations then you can emove the Volo.Abp.AspNetCore.Components.Server.BasicTheme package from the application. 
1. 
