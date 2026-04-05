using Mazer.WebAssemblyDemo.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Mazer.WebAssemblyDemo.Permissions;

public class WebAssemblyDemoPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(WebAssemblyDemoPermissions.GroupName);

        var booksPermission = myGroup.AddPermission(WebAssemblyDemoPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(WebAssemblyDemoPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(WebAssemblyDemoPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(WebAssemblyDemoPermissions.Books.Delete, L("Permission:Books.Delete"));

        //Define your own permissions here. Example:
        //myGroup.AddPermission(WebAssemblyDemoPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WebAssemblyDemoResource>(name);
    }
}
