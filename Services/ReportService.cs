using ClosedXML.Excel;
using FarmaciaApp.ViewModels; // O donde tengas ProductoDto
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FarmaciaApp.Services
{
    public class ReportService
    {
        // 1. Generación de Excel con ClosedXML
        public byte[] GenerarExcelProductos(List<ProductoDto> productos)
        {
            using var libro = new XLWorkbook();
            var hoja = libro.Worksheets.Add("Inventario Farmacia");

            // Encabezados
            hoja.Cell(1, 1).Value = "ID";
            hoja.Cell(1, 2).Value = "Producto";
            hoja.Cell(1, 3).Value = "Precio (BS)";
            hoja.Cell(1, 4).Value = "stockActual";
            hoja.Range("A1:D1").Style.Font.Bold = true;
            hoja.Range("A1:D1").Style.Fill.BackgroundColor = XLColor.FromHtml("#2572A9");
            hoja.Range("A1:D1").Style.Font.FontColor = XLColor.White;

            // Llenado de datos (Precisión decimal ODS 8)
            for (int i = 0; i < productos.Count; i++)
            {
                var row = i + 2;
                hoja.Cell(row, 1).Value = productos[i].Id;
                hoja.Cell(row, 2).Value = productos[i].Nombre;
                hoja.Cell(row, 3).Value = productos[i].Precio;
                hoja.Cell(row, 3).Style.NumberFormat.Format = "#,##0.00"; // Formato moneda/decimal
                hoja.Cell(row, 4).Value = productos[i].stockActual;
            }

            hoja.Columns().AdjustToContents(); // Ajustar ancho de columnas

            using var memoria = new MemoryStream();
            libro.SaveAs(memoria);
            return memoria.ToArray();
        }

        // 2. Generación de PDF con QuestPDF
        public byte[] GenerarPdfProductos(List<ProductoDto> productos)
        {
            return QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    
                    // Encabezado
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("FARMACORP - Reporte de Inventario").FontSize(18).Bold().FontColor("#2572A9");
                            col.Item().Text($"Fecha de emisión: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(10).Italic();
                        });
                    });

                    // Contenido / Tabla
                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);  // ID
                            columns.RelativeColumn(3);  // Nombre
                            columns.RelativeColumn(1);  // Precio
                            columns.RelativeColumn(1);  // Stock
                        });

                        // Headers de la tabla
                        table.Header(header =>
                        {
                            header.Cell().Background("#2572A9").Padding(5).Text("ID").FontColor("#FFF").Bold();
                            header.Cell().Background("#2572A9").Padding(5).Text("Producto").FontColor("#FFF").Bold();
                            header.Cell().Background("#2572A9").Padding(5).Text("Precio").FontColor("#FFF").Bold();
                            header.Cell().Background("#2572A9").Padding(5).Text("Stock").FontColor("#FFF").Bold();
                        });

                        // Filas
                        foreach (var prod in productos)
                        {
                            table.Cell().BorderBottom(1).BorderColor("#DDD").Padding(5).Text(prod.Id.ToString());
                            table.Cell().BorderBottom(1).BorderColor("#DDD").Padding(5).Text(prod.Nombre);
                            table.Cell().BorderBottom(1).BorderColor("#DDD").Padding(5).Text($"{prod.Precio:F2} Bs.");
                            table.Cell().BorderBottom(1).BorderColor("#DDD").Padding(5).Text(prod.stockActual.ToString());
                        }
                    });

                    // Pie de página
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                    });
                });
            }).GeneratePdf();
        }
    }
}