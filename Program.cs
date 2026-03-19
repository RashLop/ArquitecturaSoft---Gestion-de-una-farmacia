using ProyectoArqSoft.FactoryCreators;
using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Validaciones;
using BioquimicoEntidad = ProyectoArqSoft.Models.Bioquimico;
using MedicamentoEntidad = ProyectoArqSoft.Models.Medicamento;
using ClienteEntidad = ProyectoArqSoft.Models.Cliente;

var builder = WebApplication.CreateBuilder(args);

// Configurar logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

// Registro de creators
builder.Services.AddScoped<MedicamentoRepositoryCreator>();
builder.Services.AddScoped<BioquimicoRepositoryCreator>();
builder.Services.AddScoped<ClienteRepositoryCreator>();

// Registro de repositorios para Medicamento
builder.Services.AddScoped<IRepository<MedicamentoEntidad>>(provider =>
{
    var creator = provider.GetRequiredService<MedicamentoRepositoryCreator>();
    return creator.CreateRepo();
});

// Registro de repositorios para Bioquimico
builder.Services.AddScoped<IRepository<BioquimicoEntidad>>(provider =>
{
    var creator = provider.GetRequiredService<BioquimicoRepositoryCreator>();
    return creator.CreateRepo();
});

// Registro de repositorios para Cliente
builder.Services.AddScoped<IRepository<ClienteEntidad>>(provider =>
{
    var creator = provider.GetRequiredService<ClienteRepositoryCreator>();
    return creator.CreateRepo();
});

// Registro de validaciones
builder.Services.AddScoped<IValidacion<MedicamentoEntidad>, MedicamentoValidacion>();
builder.Services.AddScoped<IValidacion<BioquimicoEntidad>, BioquimicoFormularioValidacion>();
builder.Services.AddScoped<IValidacion<ClienteEntidad>, ClienteValidacion>();
builder.Services.AddScoped<IValidacion<string>, BioquimicoBusquedaValidacion>();

// Registro de servicios
builder.Services.AddScoped<IMedicamentoService, MedicamentoService>();
builder.Services.AddScoped<IBioquimicoService, BioquimicoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();

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
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();