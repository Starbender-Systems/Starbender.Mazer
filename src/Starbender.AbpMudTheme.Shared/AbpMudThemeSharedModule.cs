using Starbender.AbpMudTheme.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Starbender.AbpMudTheme;

[DependsOn(

    )]
public class AbpMudThemeSharedModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {

    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpMudThemeSharedModule>();
        });

        var theme = context.Configuration.GetSection("MudTheme");
        context.Services.AddMudTheme(theme);
    }
}
