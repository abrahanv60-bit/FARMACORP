using System.ComponentModel.DataAnnotations;

namespace FarmaciaApp.Models;

public class Venta
{
    public int Id { get; set; }

    public DateTime FechaVenta { get; set; } = DateTime.UtcNow;

    [Range(0, 999999.99)]
    public decimal Total { get; set; }

    [StringLength(100)]
    public string Vendedor { get; set; } = "Vendedor General";

    public virtual ICollection<DetalleVenta> DetallesVenta { get; set; } = new HashSet<DetalleVenta>();
}