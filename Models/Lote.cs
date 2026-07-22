using System.ComponentModel.DataAnnotations;

namespace FarmaciaApp.Models;

public class Lote
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string NumeroLote { get; set; } = string.Empty;

    [Required]
    public DateTime FechaVencimiento { get; set; }

    [Range(0, 100000)]
    public int StockActual { get; set; }

    public int ProductoId { get; set; }
    public virtual Producto Producto { get; set; } = null!;
}