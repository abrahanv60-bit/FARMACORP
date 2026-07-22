using System.ComponentModel.DataAnnotations;

namespace FarmaciaApp.Models;

public class Producto
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(100)]
    public string PrincipioActivo { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Categoria { get; set; } = string.Empty; // Atributo directo

    [Range(0, 999999.99)]
    public decimal Precio { get; set; }

    public virtual ICollection<Lote> Lotes { get; set; } = new HashSet<Lote>();
}