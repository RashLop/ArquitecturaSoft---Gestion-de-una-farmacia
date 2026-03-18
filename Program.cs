
using MedicamentoEntidad = ProyectoArqSoft.Models.Medicamento;
using ProyectoArqSoft.FactoryCreators;
using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Validaciones;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<MedicamentoRepositoryCreator>();
builder.Services.AddScoped<MedicamentoRepository>();

builder.Services.AddScoped<IRepository<MedicamentoEntidad>>(provider =>
{
    var creator = provider.GetRequiredService<MedicamentoRepositoryCreator>();
    return creator.CreateRepo();
});
builder.Services.AddScoped<IValidacion<MedicamentoEntidad>, MedicamentoValidacion>();
builder.Services.AddScoped<IMedicamentoService, MedicamentoService>();


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
