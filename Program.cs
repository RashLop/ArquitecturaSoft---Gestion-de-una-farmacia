using MedicamentoEntidad = ProyectoArqSoft.Models.Medicamento;
using BioquimicoEntidad = ProyectoArqSoft.Models.Bioquimico;
using ClienteEntidad = ProyectoArqSoft.Models.Cliente;
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
builder.Services.AddScoped<ClienteRepositoryCreator>();
builder.Services.AddScoped<ClienteRepository>();

builder.Services.AddScoped<IRepository<MedicamentoEntidad>>(provider =>
{
    var creator = provider.GetRequiredService<MedicamentoRepositoryCreator>();
    return creator.CreateRepo();
});
builder.Services.AddScoped<IValidacion<MedicamentoEntidad>, MedicamentoValidacion>();
builder.Services.AddScoped<IMedicamentoService, MedicamentoService>();

builder.Services.AddScoped<IRepository<ClienteEntidad>>(provider =>
{
    var creator = provider.GetRequiredService<ClienteRepositoryCreator>();
    return creator.CreateRepo();
});
builder.Services.AddScoped<IValidacion<ClienteEntidad>, ClienteValidacion>();
builder.Services.AddScoped<IClienteService, ClienteService>();

// --- CONFIGURACION BIOQUIMICO ---
builder.Services.AddScoped<IRepository<BioquimicoEntidad>>(provider =>
{
    var creator = provider.GetRequiredService<BioquimicoRepositoryCreator>();
    return creator.CreateRepo();
});

builder.Services.AddScoped<IValidacion<BioquimicoEntidad>, BioquimicoFormularioValidacion>();
builder.Services.AddScoped<IValidacion<string>, BioquimicoBusquedaValidacion>();
builder.Services.AddScoped<IBioquimicoService, BioquimicoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
app.MapRazorPages()
   .WithStaticAssets();
app.Run();
