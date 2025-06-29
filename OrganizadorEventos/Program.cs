using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OrganizadorEventos;
using OrganizadorEventos.Data;
using OrganizadorEventos.Enum;
using OrganizadorEventos.IdentityErrorDescriber;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Model;
using OrganizadorEventos.Repository;
using OrganizadorEventos.Response;
using OrganizadorEventos.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor, insira um token JWT válido",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddIdentity<Usuario, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<IdentityErrorDescriberPtBr>();

//Custos
builder.Services.AddScoped<ICustoRepository, CustoRepository>();
builder.Services.AddScoped<ICustoService, CustoService>();

//ListaCustos
builder.Services.AddScoped<IListaCustoRepository, ListaCustoRepository>();
builder.Services.AddScoped<IListaCustoService, ListaCustoService>();

//Participantes
builder.Services.AddScoped<IParticipanteListaCustoRepository, ParticipanteListaCustoRepository>();
builder.Services.AddScoped<IParticipanteListaCustoService, ParticipanteListaCustoService>();
builder.Services.AddScoped<IParticipanteService, ParticipanteService>();

//Repos
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<IContatoRepository, ContatoRepository>();
builder.Services.AddScoped<ILocalRepository, LocalRepository>();
builder.Services.AddScoped<IParticipanteRepository, ParticipanteRepository>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

//Services
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IContatoService, ContatoService>();
builder.Services.AddScoped<ILocalService, LocalService>();
builder.Services.AddScoped<IParticipanteService, ParticipanteService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRelatorioEventoService, RelatorioEventoService>();
builder.Services.AddScoped<IPixService, PixService>();
builder.Services.AddScoped<IGoogleDriveService, GoogleDriveService>();
builder.Services.AddScoped<IPagamentoParticipanteRepository, PagamentoParticipanteRepository>();
builder.Services.AddScoped<IConviteService, ConviteService>();
builder.Services.AddScoped<IPagamentoService, PagamentoService>();
builder.Services.AddHttpClient();

builder.Services.RegisterJWT(builder.Configuration);

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        var customResponse = GenericResponse<string>.ErroResponse(
            errors,
            "Erro de validação."
        );

        return new BadRequestObjectResult(customResponse);
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
}

app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();

    var userManager = services.GetRequiredService<UserManager<Usuario>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    
    await Startup.SeedDefaultUserAsync(userManager, roleManager);
    await Startup.SeedContatosAsync(context, userManager);
    await Startup.SeedLocaisAsync(context, userManager);

    foreach (var role in Enum.GetNames(typeof(UserRole)))
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole<Guid>(role));
}

app.Run();