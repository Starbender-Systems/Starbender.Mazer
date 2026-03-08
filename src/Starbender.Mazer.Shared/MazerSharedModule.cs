using Starbender.Mazer.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Starbender.Mazer;

[DependsOn(

    )]
public class MazerSharedModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {

    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<MazerSharedModule>();
        });

        var theme = context.Configuration.GetSection("MudTheme");
        context.Services.AddMudTheme(theme);
    }
}
