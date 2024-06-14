using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using TaskManagerAngular.Api.Common;
using TaskManagerAngular.Api.Data;
using TaskManagerAngular.Api.Identity;
using TaskManagerAngular.Api.Services;

namespace TaskManagerAngular.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString =
                    configuration.GetConnectionString("Database") ??
                    throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddSingleton<ISqlConnectionFactory>(_ =>
            new SqlConnectionFactory(connectionString));

        return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();
        services.AddTransient<UserManager<ApplicationUser>, ApplicationUserManager>();
        services.AddTransient<SignInManager<ApplicationUser>, ApplicationSignInManager>();
        services.AddTransient<RoleManager<ApplicationRole>, ApplicationRoleManager>();
        services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
        services.AddTransient<IUsersService, UsersService>();

        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserStore<ApplicationUserStore>()
            .AddUserManager<ApplicationUserManager>()
            .AddRoleManager<ApplicationRoleManager>()
            .AddSignInManager<ApplicationSignInManager>()
            .AddRoleStore<ApplicationRoleStore>()
            .AddDefaultTokenProviders();

        services.AddScoped<ApplicationRoleStore>();
        services.AddScoped<ApplicationUserStore>();
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        //Configure JWT Authentication
        var appSettingsSection = configuration.GetSection("AppSettings");
        services.Configure<AppSettings>(appSettingsSection);

        var appSettings = appSettingsSection.Get<AppSettings>();
        var key = System.Text.Encoding.ASCII.GetBytes(appSettings.Secret);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        //.AddCookie()
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        return services;
    }

    public static IServiceCollection AddAntiforgeryConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAntiforgery(options =>
        {
            options.Cookie.Name = "XSRF-Cookie-TOKEN";
            options.HeaderName = "X-XSRF-TOKEN";
        });

        return services;
    }

    public static async Task UseDefaultRoleGenerationAsync(this WebApplication app)
    {
        IServiceScopeFactory serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
        using (IServiceScope scope = serviceScopeFactory.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //Create Admin Role
            if ((await roleManager.RoleExistsAsync("Admin")) == false)
            {
                var role = new ApplicationRole();
                role.Name = "Admin";
                role.Description = "This is a Admin Role";
                await roleManager.CreateAsync(role);
            }

            //Create Admin User
            if ((await userManager.FindByNameAsync("admin")) == null)
            {
                var user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "admin@gmail.com";
                var userPassword = "Admin123#";
                var chkUser = await userManager.CreateAsync(user, userPassword);
                if (chkUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            if ((await userManager.IsInRoleAsync(await userManager.FindByNameAsync("admin"), "Admin")) == false)
            {
                await userManager.AddToRoleAsync(await userManager.FindByNameAsync("admin"), "Admin");
            }

            //Create Employee Role
            if ((await roleManager.RoleExistsAsync("Employee")) == false)
            {
                var role = new ApplicationRole();
                role.Name = "Employee";
                await roleManager.CreateAsync(role);
            }
        }

        return;
    }
}
