using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Starbender.AbpMudTheme;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpMudBlazorThemeInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpMudBlazorThemeInstallerModule>();
        });
    }
}
