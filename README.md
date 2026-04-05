ASpDotNetWebAPI
================

Project summary
---------------
- Minimal ASP.NET Core Web API (target: .NET 8) that includes ASP.NET Core Identity for authentication.
- Identity store uses an in-memory Identity DB (AuthDBContext). A separate ApplicationDbcontext is registered for application data using SQL Server (DefaultConnection).
- Swagger is enabled in Development.

Key facts
---------
- TargetFramework: net8.0
- Important packages (from project file):
  - Microsoft.AspNetCore.Identity.EntityFrameworkCore
  - Microsoft.EntityFrameworkCore (InMemory + SqlServer)
  - Swashbuckle.AspNetCore

Repository layout
-----------------
- Program.cs                — application composition and service registrations
- Data/AuthDBContext.cs     — IdentityDbContext<IdentityUser> (in-memory configured)
- ApplicationDbcontext      — separate DbContext (SQL Server) registered in Program.cs
- Controllers/              — Web API controllers (e.g. EmployeesController)
- ASpDotNetWebAPI.csproj    — NuGet package references

Prerequisites
-------------
- .NET 8 SDK
- (If using SQL Server features) a reachable SQL Server and a valid DefaultConnection in appsettings.json

Build & run
-----------
From repository root:
- dotnet restore
- dotnet build
- dotnet run

When running in Development, Swagger UI is available (Program.cs enables it for dev environment).

Identity / Known runtime issue
------------------------------
Symptom observed: System.InvalidOperationException: No service for type 'Microsoft.AspNetCore.Identity.IEmailSender`1[Microsoft.AspNetCore.Identity.IdentityUser]' has been registered.

Root cause: Program maps Identity UI endpoints via app.MapIdentityApi<IdentityUser>() while only registering core identity services via AddIdentityCore<IdentityUser>(). The MapIdentityApi endpoints expect additional UI-related services (for example IEmailSender<TUser>) which are not registered by AddIdentityCore. The project also doesn't include the Identity UI package by default, so no automatic registration occurs.

Quick fixes (choose one)
- Minimal/no-op (fast, for development):
  1) Add a simple generic no-op IEmailSender and register it in Program.cs before builder.Build():

     public class NoopEmailSender<TUser> : Microsoft.AspNetCore.Identity.IEmailSender<TUser> where TUser : class
     {
         public System.Threading.Tasks.Task SendEmailAsync(TUser user, string subject, string htmlMessage) => System.Threading.Tasks.Task.CompletedTask;
     }

     // registration
     builder.Services.AddSingleton(typeof(Microsoft.AspNetCore.Identity.IEmailSender<>), typeof(NoopEmailSender<>));

  This satisfies DI for the mapped Identity endpoints and avoids the exception. For production, use a real email sender.

- Full UI approach (recommended if you want built-in Identity UI behavior):
  1) Add the Microsoft.AspNetCore.Identity.UI package.
  2) Replace AddIdentityCore(...) with AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<AuthDBContext>();

  This registers the UI-related services expected by MapIdentityApi.

Notes and recommendations
-------------------------
- For a production email sender, implement IEmailSender<TUser> using an SMTP provider or a transactional email API (SendGrid, etc.). Prefer scoped/transient lifetimes for such services.
- Use AddDefaultIdentity when you plan to use the built-in Identity endpoints and UI. Use AddIdentityCore only when you are intentionally wiring up a custom identity surface and will provide all required services yourself.
- Keep Identity DbContext and application DbContext responsibilities separate and document the reason for two DbContexts.

Testing
-------
- Verify the app starts without DI errors after applying one of the fixes.
- Test registration/login endpoints (and email-confirmation flows if implemented) via Swagger or integration tests.

Contributing
------------
- Fork, implement changes on feature branches, and open a PR. Ensure builds/tests pass.

Maintainer
----------
- Repository: https://github.com/SnehaDeepakBavirisetti/ASpDotNetWebAPI

