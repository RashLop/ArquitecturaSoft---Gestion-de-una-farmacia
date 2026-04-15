using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProyectoArqSoft.Application.Facades;
using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Application.Ports.Output;
using ProyectoArqSoft.Application.Services;
using ProyectoArqSoft.Domain.DTOs;
using ProyectoArqSoft.Domain.Validators;
using ProyectoArqSoft.Infrastructure.Creadores;
using ProyectoArqSoft.Infrastructure.Persistence.Repositories;
using System.Text;

using ClasificacionEntidad = ProyectoArqSoft.Domain.Models.Clasificacion;
using ClienteEntidad = ProyectoArqSoft.Domain.Models.Cliente;
using MedicamentoEntidad = ProyectoArqSoft.Domain.Models.Medicamento;
using VentaEntidad = ProyectoArqSoft.Domain.Models.Venta;

Env.Load();

var builder = WebApplication.CreateBuilder(args);


// =========================
// CONFIGURACIÓN BASE
// =========================
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();


// =========================
// MEDICAMENTOS
// =========================
builder.Services.AddScoped<MedicamentoRepositoryCreator>();

builder.Services.AddScoped<IMedicamentoRepository, MedicamentoRepository>();

builder.Services.AddScoped<IRepository<MedicamentoEntidad>>(provider =>
{
    var creator = provider.GetRequiredService<MedicamentoRepositoryCreator>();
    return creator.CreateRepo();
});

builder.Services.AddScoped<IResult<MedicamentoEntidad>, MedicamentoValidacion>();
builder.Services.AddScoped<IMedicamentoService, MedicamentoService>();


// =========================
// CLIENTES
// =========================
builder.Services.AddScoped<ClienteRepositoryCreator>();

builder.Services.AddScoped<IRepository<ClienteEntidad>>(provider =>
{
    var creator = provider.GetRequiredService<ClienteRepositoryCreator>();
    return creator.CreateRepo();
});

builder.Services.AddScoped<IResult<ClienteEntidad>, ClienteValidacion>();
builder.Services.AddScoped<IClienteService, ClienteService>();


// =========================
// USUARIOS
// =========================
builder.Services.AddScoped<UsuarioRepositoryCreator>();
builder.Services.AddScoped<UsuarioTokenRepositoryCreator>();

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

builder.Services.AddScoped<UsuarioNegocioValidacion>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioTokenService, UsuarioTokenService>();


// =========================
// CLASIFICACIÓN
// =========================
builder.Services.AddScoped<ClasificacionRepositoryCreator>();

builder.Services.AddScoped<IRepository<ClasificacionEntidad>>(provider =>
{
    var creator = provider.GetRequiredService<ClasificacionRepositoryCreator>();
    return creator.CreateRepo();
});

builder.Services.AddScoped<IClasificacionRepository, ClasificacionRepository>();
builder.Services.AddScoped<IResult<ClasificacionEntidad>, ClasificacionValidacion>();
builder.Services.AddScoped<IClasificacionService, ClasificacionService>();


// =========================
// VENTAS
// =========================
builder.Services.AddScoped<VentaRepositoryCreator>();

builder.Services.AddScoped<IVentaRepository>(provider =>
{
    var creator = provider.GetRequiredService<VentaRepositoryCreator>();
    return creator.CreateRepo();
});

builder.Services.AddScoped<IResult<VentaEntidad>, VentaValidacion>();
builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<FachadaVenta>();
builder.Services.AddScoped<FachadaAnular>();
builder.Services.AddScoped<FachadaActualizarStock>();
builder.Services.AddScoped<IVentaFacade, VentaFacade>();


//repos
builder.Services.AddScoped<IMedicamentoRepository, MedicamentoRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IVentaRepository, VentaRepository>();
builder.Services.AddScoped<IClasificacionRepository, ClasificacionRepository>();


// =========================
// DASHBOARD / INDEX
// =========================
builder.Services.AddScoped<EstadisticasService>();
builder.Services.AddScoped<IDashboardFacade, DashboardFacade>();


// =========================
// AUTH / EMAIL / TOKEN
// =========================
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IResult<UsuarioRegistroDto>, UsuarioRegistroValidacion>();
builder.Services.AddScoped<IResult<UsuarioActualizacionDto>, UsuarioActualizacionValidacion>();
builder.Services.AddScoped<IResult<UsuarioLoginRequestDto>, UsuarioLoginRequestValidacion>();


// =========================
// JWT
// =========================
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            ),
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


// =========================
// APP
// =========================
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
