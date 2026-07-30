using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmaciaApp.Data;
using FarmaciaApp.Interfaces;
using FarmaciaApp.Models;
using FarmaciaApp.Services;
using FarmaciaApp.ViewModels;

namespace FarmaciaApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesApiController : ControllerBase
    {
        private readonly IRepository<Producto> _repository;
        private readonly ApplicationDbContext _context; // O tu DbContext para la agrupación por categoría
        private readonly ReportService _reportService;

        public ReportesApiController(
            IRepository<Producto> repository, 
            ApplicationDbContext context,
            ReportService reportService)
        {
            _repository = repository;
            _context = context;
            _reportService = reportService;
        }

        // Descargar Excel
        [HttpGet("excel")]
        public async Task<IActionResult> DescargarExcel()
        {
            var productos = await ObtenerDtos();
            var bytes = _reportService.GenerarExcelProductos(productos);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Inventario.xlsx");
        }

        // Descargar PDF
        [HttpGet("pdf")]
        public async Task<IActionResult> DescargarPdf()
        {
            var productos = await ObtenerDtos();
            var bytes = _reportService.GenerarPdfProductos(productos);
            return File(bytes, "application/pdf", "Reporte_Inventario.pdf");
        }

        // Endpoint de datos para Chart.js (Fase B)
         [HttpGet("datos-grafico")]
        public async Task<IActionResult> GetDatosGrafico()
        {
            // Agrupamos directamente por p.Categoria (ya que es un string)
            var datos = await _context.Productos
                .AsNoTracking()
                .GroupBy(p => p.Categoria ?? "Sin Categoría")
                .Select(g => new 
                { 
                    Etiqueta = g.Key, 
                    Valor = g.Count() 
                })
                .ToListAsync();

            return Ok(datos);
        }

        private async Task<List<ProductoDto>> ObtenerDtos()
        {
            var productos = await _repository.GetAllAsync();
            return productos.Select(p => new ProductoDto(
                p.Id,
                p.Nombre,
                p.Precio,
                p.Lotes != null ? p.Lotes.Sum(l => l.StockActual) : 0
            )).ToList();
        }
    }
}