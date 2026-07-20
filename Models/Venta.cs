using System.ComponentModel.DataAnnotations;

namespace FarmaciaApp.Models;

public class Venta
{
    public int Id { get; set; }

    public DateTime FechaVenta { get; set; } = DateTime.UtcNow;

    [Range(0, 999999.99)]
    public decimal Total { get; set; }

    public int UsuarioId { get; set; }
    public virtual Usuario Usuario { get; set; } = null!;

    public virtual ICollection<DetalleVenta> DetallesVenta { get; set; } = new HashSet<DetalleVenta>();
}