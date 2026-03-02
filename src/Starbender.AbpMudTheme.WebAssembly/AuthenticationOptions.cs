namespace Starbender.AbpMudTheme.WebAssembly;

public class AuthenticationOptions
{
    public string LoginUrl { get; set; } = "authentication/login";

    public string LogoutUrl { get; set; } = "authentication/logout";
}
