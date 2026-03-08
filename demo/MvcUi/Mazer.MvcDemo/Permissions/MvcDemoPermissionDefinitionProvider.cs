using Mazer.MvcDemo.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Mazer.MvcDemo.Permissions;

public class MvcDemoPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(MvcDemoPermissions.GroupName);


        var booksPermission = myGroup.AddPermission(MvcDemoPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(MvcDemoPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(MvcDemoPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(MvcDemoPermissions.Books.Delete, L("Permission:Books.Delete"));
        
        //Define your own permissions here. Example:
        //myGroup.AddPermission(MvcDemoPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<MvcDemoResource>(name);
    }
}
