using Microsoft.AspNetCore.Identity; // 1. [NUEVO] Importante para Identity y IdentityRole
using Microsoft.EntityFrameworkCore;
using FarmaciaApp.Data;
using FarmaciaApp.Interfaces;
using FarmaciaApp.Repositories;
using FarmaciaApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Registrar soporte para Controladores MVC y Vistas
builder.Services.AddControllersWithViews();

// 2. [NUEVO] Necesario para cargar las vistas predeterminadas de Login/Registro de Identity UI
builder.Services.AddRazorPages();

// Configuración de la base de datos PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

// 3. [NUEVO] Configuración de ASP.NET Core Identity (Actividad 08)
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
})
.AddRoles<IdentityRole>() // CRÍTICO: Habilita el soporte para roles (Admin y Vendedor)
.AddEntityFrameworkStores<ApplicationDbContext>();

// Inyección de dependencias del repositorio
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

// 4. [NUEVO] Data Seeding de Roles (Crea automáticamente "Admin" y "Vendedor" al arrancar)
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Admin", "Vendedor" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

// Permitir archivos estáticos (CSS, JS)
app.UseStaticFiles();

app.UseAntiforgery();

// 5. [NUEVO] Middlewares de Seguridad (ORDEN CRÍTICO)
app.UseAuthentication(); // ¿Quién eres?
app.UseAuthorization();  // ¿Qué puedes hacer?

app.MapStaticAssets();

// Mapear las rutas de los Controladores MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Productos}/{action=Index}/{id?}");

// 6. [NUEVO] Mapear las páginas de Login / Registro de Identity
app.MapRazorPages();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();