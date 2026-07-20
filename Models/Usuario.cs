using System.ComponentModel.DataAnnotations;

namespace FarmaciaApp.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string NombreUsuario { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Rol { get; set; } = "Vendedor"; // "Administrador" o "Vendedor"

    public virtual ICollection<Venta> Ventas { get; set; } = new HashSet<Venta>();
}