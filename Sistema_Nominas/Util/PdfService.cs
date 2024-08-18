using iText.Kernel.Pdf;
using iText.Layout.Element;
using Sistema_Nominas.Models;
using iText.Layout;
using System.IO;
using System.Threading.Tasks;

namespace Sistema_Nominas.Util
{
    public class PdfService
    {
        public async Task<byte[]> GenerateNominaPdfAsync(NominaViewModel model)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Título del PDF
                document.Add(new Paragraph("Recibo de Nómina")
                    .SetFontSize(20)
                    .SetBold());

                // Información de la nómina
                document.Add(new Paragraph($"Empleado: {model.NombreEmpleado}"));
                document.Add(new Paragraph($"Fecha Inicio: {model.PeriodoInicio:dd/MM/yyyy}"));
                document.Add(new Paragraph($"Fecha Fin: {model.PeriodoFin:dd/MM/yyyy}"));
                document.Add(new Paragraph($"Salario Base: {model.SalarioBase:C}"));
                document.Add(new Paragraph($"Total Ingresos: {model.TotalIngresos:C}"));
                document.Add(new Paragraph($"Total Deducciones: {model.TotalDeducciones:C}"));
                document.Add(new Paragraph($"Total Neto: {model.TotalNeto:C}"));

                // Detalles de la nómina
                document.Add(new Paragraph("Detalles:")
                    .SetFontSize(16)
                    .SetBold());

                var table = new Table(3);
                table.AddHeaderCell("Concepto");
                table.AddHeaderCell("Tipo");
                table.AddHeaderCell("Monto");

                foreach (var detalle in model.Detalles)
                {
                    table.AddCell(detalle.NombreConcepto);
                    table.AddCell(detalle.TipoConcepto);
                    table.AddCell(detalle.Monto.ToString("C"));
                }

                document.Add(table);

                document.Close();
                return stream.ToArray();
            }
        }
    }
}
