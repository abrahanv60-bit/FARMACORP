using System.ComponentModel.DataAnnotations;

namespace FarmaciaApp.Models;

public class DetalleVenta
{
    public int Id { get; set; }

    public int VentaId { get; set; }
    public virtual Venta Venta { get; set; } = null!;
    
    public int LoteId { get; set; }
    public virtual Lote Lote { get; set; } = null!;

    [Range(1, 1000)]
    public int Cantidad { get; set; }

    [Range(0, 999999.99)]
    public decimal PrecioUnitario { get; set; }

    [Range(0, 999999.99)]
    public decimal Subtotal { get; set; }
}