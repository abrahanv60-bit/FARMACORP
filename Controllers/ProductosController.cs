using Microsoft.AspNetCore.Mvc;
using FarmaciaApp.Interfaces;
using FarmaciaApp.Models;
using FarmaciaApp.ViewModels;

namespace FarmaciaApp.Controllers;

public class ProductosController : Controller
{
    private readonly IRepository<Producto> _repository;

    public ProductosController(IRepository<Producto> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var lista = await _repository.GetAllAsync();
        return View(lista);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Recibe el ViewModel en lugar de la Entidad directa
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductoCreateViewModel vm)
    {
        // 1. Validación en Servidor
        if (!ModelState.IsValid)
        {
            return View(vm); // Retorna a la vista con los mensajes de error
        }

        // 2. Mapeo del ViewModel a la Entidad de BD
        var nuevoProducto = new Producto
        {
            Nombre = vm.Nombre,
            PrincipioActivo = vm.PrincipioActivo,
            Categoria = vm.Categoria,
            Precio = vm.Precio
        };

        // 3. Persistencia
        await _repository.AddAsync(nuevoProducto);
        await _repository.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}