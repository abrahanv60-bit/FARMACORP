using System.ComponentModel.DataAnnotations;

namespace FarmaciaApp.Models;

public class Laboratorio
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    public virtual ICollection<Producto> Productos { get; set; } = new HashSet<Producto>();
}