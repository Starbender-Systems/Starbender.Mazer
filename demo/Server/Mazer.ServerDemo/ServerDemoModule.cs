using Mazer.ServerDemo.Components;
using Mazer.ServerDemo.Data;
using Mazer.ServerDemo.HealthChecks;
using Mazer.ServerDemo.Localization;
using Mazer.ServerDemo.Menus;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Starbender.Mazer.Mvc;
using Starbender.Mazer.Mvc.Bundling;
using Starbender.Mazer.Server;
using Starbender.Mazer.Server.Bundling;
using Starbender.Mazer.Extensions;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Caching;
using Volo.Abp.Emailing;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.Blazor.Server;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Blazor.Server;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.Blazor.Server;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.OpenIddict;
using Volo.Abp.Security.Claims;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.Blazor.Server;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Studio;
using Volo.Abp.Studio.Client.AspNetCore;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.Blazor.Server;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Mazer.ServerDemo;

[DependsOn(
    // ABP Framework packages

    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(AbpMapperlyModule),
    typeof(AbpCachingModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpStudioClientAspNetCoreModule),

    // Account module packages

    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpAccountApplicationModule),

    // Identity module packages

    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpPermissionManagementDomainOpenIddictModule),
    typeof(AbpIdentityBlazorServerModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpIdentityApplicationModule),

    // Tenant Management module packages

    typeof(AbpTenantManagementBlazorServerModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpTenantManagementApplicationModule),

    // Feature Management module packages

    typeof(AbpFeatureManagementBlazorServerModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpFeatureManagementApplicationModule),

    // Permission Management module packages

    typeof(AbpPermissionManagementBlazorServerModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpPermissionManagementHttpApiModule),

    // Setting Management module packages

    typeof(AbpSettingManagementBlazorServerModule),
    typeof(AbpSettingManagementHttpApiModule),
    typeof(AbpSettingManagementApplicationModule),

    // Theme module packages

    typeof(MazerMvcModule),
    typeof(MazerServerModule),

    // Entity Framework Core packages for the used modules

    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
    typeof(BlobStoringDatabaseEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule)
)]
public class ServerDemoModule : AbpModule
{
    /* Single point to enable/disable multi-tenancy */
    public const bool IsMultiTenant = true;

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(ServerDemoResource)
            );
        });

        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("ServerDemo");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });

        if (!hostingEnvironment.IsDevelopment())
        {
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
            {
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            });

            PreConfigure<OpenIddictServerBuilder>(serverBuilder =>
            {
                serverBuilder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx", configuration["AuthServer:CertificatePassPhrase"]!);
            });
        }

        PreConfigure<AbpAspNetCoreComponentsWebOptions>(options =>
        {
            options.IsBlazorWebApp = true;
        });

        ServerDemoGlobalFeatureConfigurator.Configure();
        ServerDemoModuleExtensionConfigurator.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        // Add services to the container.
        context.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        if (hostingEnvironment.IsDevelopment())
        {
            context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
        }

        ConfigureStudio(hostingEnvironment);
        ConfigureAuthentication(context);
        ConfigureBundles();
        ConfigureBlazorise(context);
        ConfigureMudBlazor(context);

        ConfigureRouter(context);
        ConfigureMultiTenancy();
        ConfigureMenu(context);
        ConfigureUrls(configuration);
        ConfigureHealthChecks(context);
        ConfigureSwagger(context.Services);
        ConfigureAutoApiControllers();
        ConfigureLocalization();
        ConfigureVirtualFiles(hostingEnvironment);
        ConfigureEfCore(context);
    }

    private void ConfigureHealthChecks(ServiceConfigurationContext context)
    {
        context.Services.AddServerDemoHealthChecks();
    }

    private void ConfigureStudio(IHostEnvironment hostingEnvironment)
    {
        if (hostingEnvironment.IsProduction())
        {
            Configure<AbpStudioClientOptions>(options =>
            {
                options.IsLinkEnabled = false;
            });
        }
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                MazerBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );

            options.ScriptBundles.Configure(
                MazerBundles.Scripts.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-scripts.js");
                }
            );

            options.StyleBundles.Configure(
               BlazorMazerBundles.Styles.Global,
               bundle =>
               {
                   bundle.AddFiles("/global-styles.css");
               }
            );
        });
    }

    private void ConfigureBlazorise(ServiceConfigurationContext context)
    {

        context.Services
            .AddBootstrap5Providers()   // Leave this (for now)
            .AddFontAwesomeIcons();     // Leave this (for now)
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

        // You can load a theme from the configuration
        context.Services.AddMudTheme(context.Configuration.GetSection("MudTheme"));

        // You can override the Mazer here
        //Configure<MudTheme>(theme =>
        //{
        //    theme.LayoutProperties.DrawerWidthLeft = "200px";
        //});
    }

    private void ConfigureRouter(ServiceConfigurationContext _)
    {
        Configure<AbpRouterOptions>(options =>
        {
            options.AppAssembly = typeof(ServerDemoModule).Assembly;
        });
    }

    private void ConfigureMultiTenancy()
    {
        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = IsMultiTenant;
        });
    }

    private void ConfigureMenu(ServiceConfigurationContext _)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new ServerDemoMenuContributor());
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());
        });
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<ServerDemoResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource), typeof(AbpUiResource))
                .AddVirtualJson("/Localization/ServerDemo");

            options.DefaultResourceType = typeof(ServerDemoResource);

            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("ar", "ar", "Arabic"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "Chinese (Simplified)"));
            options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "Chinese (Traditional)"));
            options.Languages.Add(new LanguageInfo("cs", "cs", "Czech"));
            options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (United Kingdom)"));
            options.Languages.Add(new LanguageInfo("fi", "fi", "Finnish"));
            options.Languages.Add(new LanguageInfo("fr", "fr", "French"));
            options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "German (Germany)"));
            options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi "));
            options.Languages.Add(new LanguageInfo("hu", "hu", "Hungarian"));
            options.Languages.Add(new LanguageInfo("is", "is", "Icelandic"));
            options.Languages.Add(new LanguageInfo("it", "it", "Italian"));
            options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Portuguese (Brazil)"));
            options.Languages.Add(new LanguageInfo("ro-RO", "ro-RO", "Romanian (Romania)"));
            options.Languages.Add(new LanguageInfo("ru", "ru", "Russian"));
            options.Languages.Add(new LanguageInfo("sk", "sk", "Slovak"));
            options.Languages.Add(new LanguageInfo("es", "es", "Spanish"));
            options.Languages.Add(new LanguageInfo("sv", "sv", "Swedish"));
            options.Languages.Add(new LanguageInfo("tr", "tr", "Turkish"));

        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("ServerDemo", typeof(ServerDemoResource));
        });
    }

    private void ConfigureAutoApiControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(ServerDemoModule).Assembly);
        });
    }

    private void ConfigureSwagger(IServiceCollection services)
    {
        services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "ServerDemo API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            }
        );
    }

    private void ConfigureVirtualFiles(IWebHostEnvironment hostingEnvironment)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ServerDemoModule>();
            if (hostingEnvironment.IsDevelopment())
            {
                /* Using physical files in development, so we don't need to recompile on changes */
                options.FileSets.ReplaceEmbeddedByPhysical<ServerDemoModule>(hostingEnvironment.ContentRootPath);
            }
        });
    }

    private void ConfigureEfCore(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ServerDemoDbContext>(options =>
        {
            /* You can remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots
             * Documentation: https://docs.abp.io/en/abp/latest/Entity-Framework-Core#add-default-repositories
             */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(configurationContext =>
            {
                configurationContext.UseSqlServer();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.UseAbpSecurityHeaders();
        app.UseRouting();
        app.MapAbpStaticAssets();
        app.UseAbpStudioLink();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (IsMultiTenant)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAntiforgery();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "ServerDemo API");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints(builder =>
        {
            builder.MapRazorPages();
            builder.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddAdditionalAssemblies(builder.ServiceProvider.GetRequiredService<IOptions<AbpRouterOptions>>().Value.AdditionalAssemblies.ToArray());
        });
    }
}
