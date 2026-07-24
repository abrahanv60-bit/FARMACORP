using FarmaciaApp.Interfaces;
using FarmaciaApp.Repositories;
using FarmaciaApp.Components;
using Microsoft.EntityFrameworkCore;
using FarmaciaApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 1. [NUEVO] Registrar soporte para Controladores MVC y Vistas (Actividad 06)
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

// Inyección de dependencias del repositorio
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

// 2. [NUEVO] Permitir archivos estáticos (CSS, JS)
app.UseStaticFiles();

app.UseAntiforgery();

app.MapStaticAssets();

// 3. [NUEVO] Mapear las rutas de los Controladores MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Productos}/{action=Index}/{id?}");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();