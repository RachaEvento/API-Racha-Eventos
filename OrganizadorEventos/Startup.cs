using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OrganizadorEventos.Model;

namespace OrganizadorEventos;

public static class Startup
{
    public static async Task SeedDefaultUserAsync(UserManager<Usuario> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        var baseUserName = "User";
        var baseUserEmail = "user@mail.com";
        var baseUserPassword = "User@123";

        if (await userManager.FindByEmailAsync(baseUserEmail) == null)
        {
            var baseUser = new Usuario
            {
                UserName = baseUserName,
                Email = baseUserEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(baseUser, baseUserPassword);
            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync("Usuario"))
                    await roleManager.CreateAsync(new IdentityRole<Guid>("Usuario"));

                await userManager.AddToRoleAsync(baseUser, "Usuario");
            }
        }
    }

    public static void RegisterJWT(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(authOptions =>
        {
            authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearerOptions =>
        {
            bearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
        });

        services.AddAuthorization(auth =>
        {
            auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().Build());
        });
    }
}