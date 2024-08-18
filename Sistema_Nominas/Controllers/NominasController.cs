using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Nominas.Models;
using Sistema_Nominas.Util;
using System.Data.SqlClient;
using System.Reflection;

namespace Sistema_Nominas.Controllers
{
    public class NominasController : Controller
    {
        private readonly PdfService _pdfService;
        // GET: NominasController
        public ActionResult Nominas()
        {
            return View();
        }
        public ActionResult AdministracionNominal()
        {
            return View();
        }
        public ActionResult MenuPrincipal()
        {
            return View();
        }
		public ActionResult Generar_Nominas()
		{
            List<NominaViewModel> list = NonimaDAL.ObtenerDatosNomina();

            return View(list);
		}
        public ActionResult ReciboView()
        {
            return View();
        }
        public ActionResult AgregarNomina()
        {
            var now = DateTime.Now;
            int maxDaysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
            var fechaGeneracion = new DateTime(now.Year, now.Month, 28);

            // Ajustar la fecha si el día 28 no existe en el mes actual (por ejemplo, para febrero en años no bisiestos)
            if (28 > DateTime.DaysInMonth(now.Year, now.Month))
            {
                fechaGeneracion = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
            }

            var model = new NominaEmpleadoModel
            {
                // Inicializa el modelo si es necesario
                DetalleNomina = new List<DetalleNominaViewModel>(),
                PeriodoInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                PeriodoFin = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)),
                FechaGeneracion = fechaGeneracion
            };

            var empleados = EmpleadosDAL.ObtenerTodosLosEmpleados(ConexionString.Conexion);

            ViewBag.Conceptos = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Ingreso" },
                new SelectListItem { Value = "2", Text = "Deducción" }
            };
            ViewBag.Empleados = new SelectList(empleados, "EmpleadoID", "Nombre");
            return View(model);
        }
        [HttpPost]
        public JsonResult CrearConceptoNominal(ConceptoNominaModel concepto)
        {
            Console.WriteLine("CrearConceptoNominal");
            Boolean resultado = false;
            try
            {
                resultado = ConceptoNominaDAL.AgregarNuevoConcepto(concepto);
                if (resultado)
                {
                    return Json(new { success = true, message = "Concepto Nominal creado correctamente" });
                }
                else
                {
                    return Json(new { success = false, message = "Concepto nominal no creado" });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CrearConceptoNominal tuvo error " + e.Message);
                throw;
            }

        }

        [HttpPost]
        public JsonResult CrearNominalEmpleado(NominaEmpleadoModel concepto)
        {
            Console.WriteLine("Nominal Empleado");
            Boolean resultado = false;
            try
            {
                Console.WriteLine($"PeriodoInicio: {concepto.PeriodoInicio}");
                Console.WriteLine($"PeriodoFin: {concepto.PeriodoFin}");
                Console.WriteLine($"FechaGeneracion: {concepto.FechaGeneracion}");
                resultado = ConceptoNominaDAL.AgregarNominaEmpleado(concepto);
                Console.WriteLine("Nominal Empleado resultado " + resultado);
                if (resultado)
                {
                    return Json(new { success = true, message = "Nominal creado correctamente" });
                }
                else
                {
                    return Json(new { success = false, message = "Nominal no creado" });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CrearConceptoNominal tuvo error " + e.Message);
                throw;
            }

        }

        [HttpPost]
        public JsonResult GenerarNominas()
        {
            try
            {
                // Call the method that generates the payroll
                ConceptoNominaDAL.GenerarNominas();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private async Task<NominaViewModel> ObtenerDatosNominaAsync(int nominaID)
        {
            var connectionString = ConexionString.Conexion;
            var model = new NominaViewModel();
            var detalles = new List<DetallesNominaViewModel>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Obtener la nómina
                using (var command = new SqlCommand(@"SELECT E.Nombre AS NombreEmpleado,N.PeriodoInicio, N.NominaID,
                                                    N.PeriodoFin,N.SalarioBase,N.TotalIngresos, 
                                                    N.TotalDeducciones,N.TotalNeto
                                                    FROM Nominas N
                                                    JOIN Empleados E ON N.EmpleadoID = E.EmpleadoID
                                                    WHERE N.NominaID = @NominaID", connection))
                {
                    command.Parameters.AddWithValue("@NominaID", nominaID);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
							model.NominaID = reader.GetInt32(reader.GetOrdinal("NominaID"));
							model.NombreEmpleado = reader.GetString(reader.GetOrdinal("NombreEmpleado"));
                            model.PeriodoInicio = reader.GetDateTime(reader.GetOrdinal("PeriodoInicio"));
                            model.PeriodoFin = reader.GetDateTime(reader.GetOrdinal("PeriodoFin"));
                            model.SalarioBase = reader.GetDecimal(reader.GetOrdinal("SalarioBase"));
                            model.TotalIngresos = reader.GetDecimal(reader.GetOrdinal("TotalIngresos"));
                            model.TotalDeducciones = reader.GetDecimal(reader.GetOrdinal("TotalDeducciones"));
                            model.TotalNeto = reader.GetDecimal(reader.GetOrdinal("TotalNeto"));
                        }
                    }
                }

                // Obtener los detalles de la nómina
                using (var command = new SqlCommand(@"SELECT CN.Nombre AS NombreConcepto, 
                                                    CN.Tipo AS TipoConcepto, 
                                                    DN.Monto
                                                FROM DetalleNomina DN
                                                JOIN ConceptosNomina CN ON DN.ConceptoID = CN.ConceptoID
                                                WHERE DN.NominaID = @NominaID", connection))
                {
                    command.Parameters.AddWithValue("@NominaID", nominaID);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var detalle = new DetallesNominaViewModel
                            {
                                NombreConcepto = reader.GetString(reader.GetOrdinal("NombreConcepto")),
                                TipoConcepto = reader.GetString(reader.GetOrdinal("TipoConcepto")),
                                Monto = reader.GetDecimal(reader.GetOrdinal("Monto"))
                            };
                            detalles.Add(detalle);
                        }
                    }
                }
            }

            model.Detalles = detalles;
            return model;
        }
        public async Task<IActionResult> DescargarRecibo(int nominaID)
        {
            // Obtiene el modelo de datos de la nómina usando el ID
            var model = await ObtenerDatosNominaAsync(nominaID);

            // Genera el PDF con el modelo de datos obtenido
            var pdfBytes = await _pdfService.GenerateNominaPdfAsync(model);

            // Retorna el archivo PDF al cliente
            return File(pdfBytes, "application/pdf", "ReciboNomina.pdf");
        }




    }/**/
}
