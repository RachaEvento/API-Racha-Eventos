using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OrganizadorEventos.Data;
using OrganizadorEventos.Model;

namespace OrganizadorEventos;

public static class Startup
{
    public static async Task SeedDefaultUserAsync(UserManager<Usuario> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        var nome = "Usuario Teste";
        var baseUserName = "User";
        var baseUserEmail = "user@mail.com";
        var baseUserPassword = "User@123";

        if (await userManager.FindByEmailAsync(baseUserEmail) == null)
        {
            var baseUser = new Usuario
            {
                Name = nome,
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
    
    public static async Task SeedContatosAsync(AppDbContext context, UserManager<Usuario> userManager)
    {
        var usuario = await userManager.FindByEmailAsync("user@mail.com");

        if (usuario != null && !context.Contatos.Any(c => c.UsuarioId == usuario.Id))
        {
            var contatos = new List<Contato> 
            { 
                new Contato
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    UsuarioId = usuario.Id,
                    Nome = "Miguel",
                    Email = "rachaeventos@gmail.com",
                    Telefone = "11999990001",
                    Ativo = true,
                    Foto = null
                },
                new Contato
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    UsuarioId = usuario.Id,
                    Nome = "Débora",
                    Email = "debora@email.com",
                    Telefone = "11999990002",
                    Ativo = true,
                    Foto = null
                },
                new Contato
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    UsuarioId = usuario.Id,
                    Nome = "Amanda",
                    Email = "amanda@email.com",
                    Telefone = "11999990003",
                    Ativo = true,
                    Foto = null
                },
                new Contato
                {
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    UsuarioId = usuario.Id,
                    Nome = "Diogo",
                    Email = "diogo@email.com",
                    Telefone = "11999990004",
                    Ativo = true,
                    Foto = null
                }
            };
            
            await context.Contatos.AddRangeAsync(contatos);
            await context.SaveChangesAsync();
        }
    }
    
    public static async Task SeedLocaisAsync(AppDbContext context, UserManager<Usuario> userManager)
    {
        var usuario = await userManager.FindByEmailAsync("user@mail.com");

        if (usuario != null && !context.Locais.Any(l => l.UsuarioId == usuario.Id))
        {
            var locais = new List<Local>
            {
                new Local
                {
                    Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                    UsuarioId = usuario.Id,
                    Nome = "Salão de festas do meu AP",
                    DescricaoLocal = "Ed. Construido",
                    Endereco = "Rua das Flores, 123",
                    Bairro = "Centro",
                    Cidade = "Criciúma",
                    Estado = "SC",
                    Ativo = true
                },
                new Local
                {
                    Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                    UsuarioId = usuario.Id,
                    Nome = "Auditório da firma",
                    DescricaoLocal = "Local de reuniões e eventos",
                    Endereco = "Av. Brasil, 456",
                    Bairro = "Jardins",
                    Cidade = "Criciúma",
                    Estado = "SC",
                    Ativo = true
                }
            };

            await context.Locais.AddRangeAsync(locais);
            await context.SaveChangesAsync();
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