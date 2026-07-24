using System.ComponentModel.DataAnnotations;

namespace FarmaciaApp.Models;

public class Producto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre no puede exceder los 150 caracteres.")]
    [Display(Name = "Nombre del Producto")]
    public string Nombre { get; set; } = string.Empty;

    [Display(Name = "Principio Activo")]
    [StringLength(100)]
    public string PrincipioActivo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La categoría es obligatoria.")]
    [StringLength(50)]
    [Display(Name = "Categoría")]
    public string Categoria { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.10, 999999.99, ErrorMessage = "El precio debe ser un valor positivo.")]
    [Display(Name = "Precio (Bs.)")]
    public decimal Precio { get; set; }

    public virtual ICollection<Lote> Lotes { get; set; } = new HashSet<Lote>();
}