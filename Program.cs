using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MedicamentoEntidad = ProyectoArqSoft.Domain.Models.Medicamento;
using ClienteEntidad = ProyectoArqSoft.Domain.Models.Cliente;
using ClasificacionEntidad = ProyectoArqSoft.Domain.Models.Clasificacion;
using VentaEntidad = ProyectoArqSoft.Domain.Models.Venta;
using ProyectoArqSoft.Domain.DTOs;
using ProyectoArqSoft.Infrastructure.Creadores;
using ProyectoArqSoft.Application.Ports.Output;
using ProyectoArqSoft.Infrastructure.Persistence.Repositories;
using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Domain.Validators;
using ProyectoArqSoft.Application.Services;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<MedicamentoRepositoryCreator>();
builder.Services.AddScoped<MedicamentoRepository>();
builder.Services.AddScoped<ClienteRepositoryCreator>();
builder.Services.AddScoped<ClienteRepository>();
builder.Services.AddScoped<UsuarioRepositoryCreator>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<ClasificacionRepositoryCreator>();
builder.Services.AddScoped<ClasificacionRepository>();
//Estadisticas
builder.Services.AddScoped<EstadisticasService>();

// Si tienes creator de token:
builder.Services.AddScoped<UsuarioTokenRepositoryCreator>();
builder.Services.AddScoped<UsuarioTokenRepository>();

builder.Services.AddScoped<IRepository<MedicamentoEntidad>>(provider =>
{
    var creator = provider.GetRequiredService<MedicamentoRepositoryCreator>();
    return creator.CreateRepo();
});
builder.Services.AddScoped<IResult<MedicamentoEntidad>, MedicamentoValidacion>();
builder.Services.AddScoped<IMedicamentoService, MedicamentoService>();

builder.Services.AddScoped<IRepository<ClienteEntidad>>(provider =>
{
    var creator = provider.GetRequiredService<ClienteRepositoryCreator>();
    return creator.CreateRepo();
});
builder.Services.AddScoped<IResult<ClienteEntidad>, ClienteValidacion>();
builder.Services.AddScoped<IClienteService, ClienteService>();


builder.Services.AddScoped<IUsuarioRepository>(provider =>
{
    var creator = provider.GetRequiredService<UsuarioRepositoryCreator>();
    return creator.CreateRepo();
});

builder.Services.AddScoped<IUsuarioTokenRepository>(provider =>
{
    var creator = provider.GetRequiredService<UsuarioTokenRepositoryCreator>();
    return creator.CreateRepo();
});

// DI Ventas
builder.Services.AddScoped<VentaRepositoryCreator>();
builder.Services.AddScoped<VentaRepository>();

builder.Services.AddScoped<IVentaRepository>(provider =>
{
    var creator = provider.GetRequiredService<VentaRepositoryCreator>();
    return creator.CreateRepo();
});

builder.Services.AddScoped<IResult<VentaEntidad>, VentaValidacion>();
builder.Services.AddScoped<IVentaService, VentaService>();

// Si NO tienes creator de token, usa esta en vez del bloque de arriba:
// builder.Services.AddScoped<IUsuarioTokenRepository, UsuarioTokenRepository>();

builder.Services.AddScoped<IResult<UsuarioRegistroDto>, UsuarioRegistroValidacion>();
builder.Services.AddScoped<IResult<UsuarioActualizacionDto>, UsuarioActualizacionValidacion>();
builder.Services.AddScoped<IResult<UsuarioLoginRequestDto>, UsuarioLoginRequestValidacion>();

builder.Services.AddScoped<IRepository<ClasificacionEntidad>>(provider =>
{
    var creator = provider.GetRequiredService<ClasificacionRepositoryCreator>();
    return creator.CreateRepo();
});
builder.Services.AddScoped<IResult<ClasificacionEntidad>, ClasificacionValidacion>();
builder.Services.AddScoped<IClasificacionService, ClasificacionService>();
builder.Services.AddScoped<IClasificacionRepository, ClasificacionRepository>();

builder.Services.AddScoped<UsuarioNegocioValidacion>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioTokenService, UsuarioTokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

string jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? string.Empty;
string jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? string.Empty;
string jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? string.Empty;

if (string.IsNullOrWhiteSpace(jwtKey))
    throw new Exception("No se encontró JWT_KEY en el archivo .env.");

if (string.IsNullOrWhiteSpace(jwtIssuer))
    throw new Exception("No se encontró JWT_ISSUER en el archivo .env.");

if (string.IsNullOrWhiteSpace(jwtAudience))
    throw new Exception("No se encontró JWT_AUDIENCE en el archivo .env.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.HttpContext.Session.GetString("Token");

                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();