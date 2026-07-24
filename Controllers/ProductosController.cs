using Microsoft.AspNetCore.Mvc;
using FarmaciaApp.Interfaces;
using FarmaciaApp.Models;

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

    [HttpPost]
    [ValidateAntiForgeryToken] // Protección contra ataques CSRF
    public async Task<IActionResult> Create(Producto modelo)
    {
        if (ModelState.IsValid) // Validación basada en Data Annotations
        {
            await _repository.AddAsync(modelo);
            await _repository.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); // Feedback de usuario: redirige al listado
        }
        
        return View(modelo); // Si falla la validación, devuelve el modelo con sus errores
    }
}