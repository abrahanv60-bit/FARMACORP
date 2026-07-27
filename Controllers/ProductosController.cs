using Microsoft.AspNetCore.Authorization; // [NUEVO]
using Microsoft.AspNetCore.Mvc;
using FarmaciaApp.Interfaces;
using FarmaciaApp.Models;
using FarmaciaApp.ViewModels;

namespace FarmaciaApp.Controllers;

[Authorize] // Bloquea acceso anónimo: exige haber iniciado sesión
public class ProductosController : Controller
{
    private readonly IRepository<Producto> _repository;

    public ProductosController(IRepository<Producto> repository)
    {
        _repository = repository;
    }

    // Accesible para cualquier usuario logueado (Admin o Vendedor)
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var lista = await _repository.GetAllAsync();
        return View(lista);
    }

    // SOLO el Admin puede ver el formulario de registro
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // SOLO el Admin puede guardar un nuevo producto
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductoCreateViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var nuevoProducto = new Producto
        {
            Nombre = vm.Nombre,
            PrincipioActivo = vm.PrincipioActivo,
            Categoria = vm.Categoria,
            Precio = vm.Precio
        };

        await _repository.AddAsync(nuevoProducto);
        await _repository.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}