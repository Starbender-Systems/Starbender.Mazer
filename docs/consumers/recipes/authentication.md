# Preserve authentication while adopting Mazer

Mazer renders account layouts, user controls, and redirects around the application's ABP authentication setup. Keep its cookie/OIDC configuration, identity provider, middleware, callback URLs, return-URL validation, and server authorization. Select the [hosting recipe](README.md) before changing theme modules or bundles.

| UI host | Mazer integration behavior | Application responsibility |
| --- | --- | --- |
| MVC | `Starbender.Mazer.Mvc.StarbenderTheme` supplies Account and Application layouts; the main toolbar includes an authenticated user menu | Preserve account routes and add an anonymous login link if required |
| Server | `Starbender.Mazer.Server.LoginDisplay` uses the ABP user menu | Preserve the authenticated circuit and server login/logout endpoints |
| WebAssembly | `MazerWebAssemblyModule` adds its assembly to ABP router additional assemblies, including `/authentication/{action}` | Preserve the application's remote authentication setup and registered callback URLs |
| WebApp | With `AbpAspNetCoreComponentsWebOptions.IsBlazorWebApp = true`, the WASM module configures `Account/Login` and `Account/Logout` paths and its redirect uses a full navigation | Preserve the server account flow, client services, and flags in both projects |

Blazor's base `Starbender.Mazer.RedirectToLogin` navigates to `account/login` with an escaped current-URI return URL and `forceLoad: true`. The WebAssembly replacement uses ABP's configured login URL for Blazor WebApp; otherwise it invokes remote-authentication navigation. Do not set `IsBlazorWebApp` simply because an application contains a WASM project: use its actual hosting/authentication model. The [WebApp recipe](blazor-webapp.md) shows the preconfiguration used by the demo.

For remote WebAssembly authentication, the library's `RemoteAuthenticatorView` page reinitializes ABP's cached application configuration after successful login and logout. Retain ABP additional assemblies in a custom router, as shown in the [layout recipe](blazor-layout.md), so that route can be discovered. For Server, user-menu links beginning with `~/` are normalized; links targeting `_blank` are opened through JS. Preserve the application's URL and target policies when contributing items.

Acceptance requires a configured identity environment: anonymous protected-route navigation, login callback and return location, authorized and forbidden content, localized account UI, logout, and subsequent anonymous navigation. For WebApp, verify both initial server rendering and later WASM interactivity. A compiled recipe or an account layout does not prove these flows work. Diagnose a missing callback route, wrong hosting flag, lost `AuthenticationStateProvider`, or mismatched return URL before rewriting authentication.
