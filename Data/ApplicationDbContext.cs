using Microsoft.EntityFrameworkCore;
using FarmaciaApp.Models;

namespace FarmaciaApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // LISTA DE TABLAS (DbSet)
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Laboratorio> Laboratorios { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Lote> Lotes { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Venta> Ventas { get; set; }
    public DbSet<DetalleVenta> DetallesVenta { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // CONFIGURACIÓN DE PRECISIÓN DECIMAL (ODS 8: Evita errores de redondeo financiero)
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var decimalProperties = entityType.GetProperties()
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?));

            foreach (var property in decimalProperties)
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }
        }
    }
}