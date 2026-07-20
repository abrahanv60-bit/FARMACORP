using System.ComponentModel.DataAnnotations;

namespace FarmaciaApp.Models;

public class Producto
{
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(100)]
    public string PrincipioActivo { get; set; } = string.Empty;

    [Range(0, 999999.99)]
    public decimal Precio { get; set; } // ODS 8: Precisión para dinero

    // Claves Foráneas y Navegación
    public int CategoriaId { get; set; }
    public virtual Categoria Categoria { get; set; } = null!;

    public int LaboratorioId { get; set; }
    public virtual Laboratorio Laboratorio { get; set; } = null!;

    // Un producto puede tener múltiples lotes con diferentes fechas de vencimiento
    public virtual ICollection<Lote> Lotes { get; set; } = new HashSet<Lote>();
}