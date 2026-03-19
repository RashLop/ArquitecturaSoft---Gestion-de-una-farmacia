
using MedicamentoEntidad = ProyectoArqSoft.Models.Medicamento;
using BioquimicoEntidad = ProyectoArqSoft.Models.Bioquimico;
using ProyectoArqSoft.FactoryCreators;
using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Validaciones;
using ProyectoArqSoft.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<MedicamentoRepositoryCreator>();
builder.Services.AddScoped<MedicamentoRepository>();
builder.Services.AddScoped<BioquimicoRepositoryCreator>();
builder.Services.AddScoped<BioquimicoRepository>();

builder.Services.AddScoped<IRepository<MedicamentoEntidad>>(provider =>
{
    var creator = provider.GetRequiredService<MedicamentoRepositoryCreator>();
    return creator.CreateRepo();
});
builder.Services.AddScoped<IValidacion<MedicamentoEntidad>, MedicamentoValidacion>();
builder.Services.AddScoped<IMedicamentoService, MedicamentoService>();

// --- CONFIGURACIÓN BIOQUÍMICO ---

// Registro del repositorio a través de su Factory (Creator)
builder.Services.AddScoped<IRepository<BioquimicoEntidad>>(provider =>
{
    var creator = provider.GetRequiredService<BioquimicoRepositoryCreator>();
    return creator.CreateRepo();
});

// Registro de la Validación (Si usas la interfaz IValidacion para Bioquímico)
builder.Services.AddScoped<IValidacion<BioquimicoEntidad>, BioquimicoFormularioValidacion>();
builder.Services.AddScoped<IValidacion<string>, BioquimicoBusquedaValidacion>();

// Registro del Servicio
builder.Services.AddScoped<IBioquimicoService, BioquimicoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.Run();
