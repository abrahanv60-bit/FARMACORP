using System.ComponentModel.DataAnnotations;

namespace FarmaciaApp.Models;

public class Categoria
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
    [StringLength(50)]
    public string Nombre { get; set; } = string.Empty;

    // Propiedad de Navegación: Una categoría contiene muchos productos
    public virtual ICollection<Producto> Productos { get; set; } = new HashSet<Producto>();
}