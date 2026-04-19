using ProyectoArqSoft.Domain.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ProyectoArqSoft.Application.Services
{
    public class ComprobanteVentaPdfService
    {
        public byte[] Generar(ComprobanteVentaPdfDto comprobante)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(25);

                    page.Header().Column(column =>
                    {
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Column(left =>
                            {
                                left.Item().Text("FARMACIA").FontSize(18).Bold();
                                left.Item().Text("Sistema de Gestión de Farmacia")
                                    .FontSize(10)
                                    .FontColor(Colors.Grey.Darken2);
                            });

                            row.RelativeItem().AlignRight().Column(right =>
                            {
                                right.Item().AlignRight()
                                    .Text("COMPROBANTE DE VENTA")
                                    .FontSize(20)
                                    .Bold()
                                    .FontColor(Colors.Teal.Darken2);

                                right.Item().AlignRight()
                                    .Text($"Fecha: {comprobante.Fecha:dd/MM/yyyy HH:mm:ss}")
                                    .FontSize(10);
                            });
                        });

                        column.Item().PaddingTop(10)
                            .LineHorizontal(1)
                            .LineColor(Colors.Grey.Lighten1);
                    });

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Spacing(12);

                        column.Item().Background(Colors.Grey.Lighten4).Padding(12).Column(info =>
                        {
                            info.Spacing(5);
                            info.Item().Text($"CI / NIT: {comprobante.Nit}").FontSize(11);
                            info.Item().Text($"Razón Social: {comprobante.RazonSocial}").FontSize(11);
                            info.Item().Text($"Cajero: {comprobante.Cajero}").FontSize(11);
                        });

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(70);
                                columns.RelativeColumn();
                                columns.ConstantColumn(100);
                                columns.ConstantColumn(100);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Teal.Darken2).Padding(6)
                                    .Text("Cantidad").FontColor(Colors.White).Bold().AlignCenter();

                                header.Cell().Background(Colors.Teal.Darken2).Padding(6)
                                    .Text("Descripción").FontColor(Colors.White).Bold().AlignCenter();

                                header.Cell().Background(Colors.Teal.Darken2).Padding(6)
                                    .Text("P. Unitario").FontColor(Colors.White).Bold().AlignCenter();

                                header.Cell().Background(Colors.Teal.Darken2).Padding(6)
                                    .Text("Importe").FontColor(Colors.White).Bold().AlignCenter();
                            });

                            foreach (var item in comprobante.Detalles)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6)
                                    .Text(item.Cantidad.ToString()).AlignCenter();

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6)
                                    .Text(item.Descripcion);

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6)
                                    .AlignRight().Text($"{item.PrecioUnitario:0.00}");

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6)
                                    .AlignRight().Text($"{item.Importe:0.00}");
                            }
                        });

                        column.Item().AlignRight().PaddingTop(10).Width(220)
                            .Background(Colors.Teal.Lighten5).Padding(10).Row(row =>
                            {
                                row.RelativeItem().Text("TOTAL Bs.").Bold().FontSize(13);
                                row.ConstantItem(80).AlignRight()
                                    .Text($"{comprobante.Total:0.00}")
                                    .Bold()
                                    .FontSize(13)
                                    .FontColor(Colors.Teal.Darken3);
                            });
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Gracias por su compra. ").FontSize(9);
                        text.Span("Documento generado por el sistema.")
                            .FontSize(9)
                            .FontColor(Colors.Grey.Darken1);
                    });
                });
            }).GeneratePdf();
        }
    }
}