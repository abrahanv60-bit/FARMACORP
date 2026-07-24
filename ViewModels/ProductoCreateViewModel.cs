using System.ComponentModel.DataAnnotations;

namespace FarmaciaApp.ViewModels;

public class ProductoCreateViewModel
{
    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 150 caracteres.")]
    [Display(Name = "Nombre del Producto")]
    public string Nombre { get; set; } = string.Empty;

    [Display(Name = "Principio Activo")]
    [StringLength(100, ErrorMessage = "El principio activo no puede exceder los 100 caracteres.")]
    public string PrincipioActivo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La categoría es obligatoria.")]
    [StringLength(50, ErrorMessage = "La categoría no puede exceder los 50 caracteres.")]
    [Display(Name = "Categoría")]
    public string Categoria { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.10, 999999.99, ErrorMessage = "El precio debe ser un valor positivo.")]
    [Display(Name = "Precio (Bs.)")]
    public decimal Precio { get; set; }
}