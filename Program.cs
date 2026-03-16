using ProyectoArqSoft.Application.Factories;
using ProyectoArqSoft.Application.Services;
using ProyectoArqSoft.Domain.Interfaces;
using ProyectoArqSoft.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

//se hace Dependecy inyection(D) en program
builder.Services.AddScoped<IMedicamentoFactory, MedicamentoFactory>();
builder.Services.AddScoped<IMedicamentoRepository, MedicamentoRepository>();
builder.Services.AddScoped<MedicamentoService>();
// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

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
