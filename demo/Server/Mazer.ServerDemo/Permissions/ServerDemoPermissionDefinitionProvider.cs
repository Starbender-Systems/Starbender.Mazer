using Mazer.ServerDemo.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Mazer.ServerDemo.Permissions;

public class ServerDemoPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ServerDemoPermissions.GroupName);


        var booksPermission = myGroup.AddPermission(ServerDemoPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(ServerDemoPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(ServerDemoPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(ServerDemoPermissions.Books.Delete, L("Permission:Books.Delete"));

        //Define your own permissions here. Example:
        //myGroup.AddPermission(ServerDemoPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ServerDemoResource>(name);
    }
}
