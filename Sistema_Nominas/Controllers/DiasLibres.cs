using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Nominas.Models;
using Sistema_Nominas.Util;
using System.Data.SqlClient;
using System.Data;

namespace Sistema_Nominas.Controllers
{
    public class DiasLibres : Controller
    {

        public IActionResult AsignarDiasLibres()
        {
            var model = new EmpleadoDiasLibresViewModel
            {
                Empleados = GetEmpleados(),
                DiasSemana = new List<string> { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" }
            };
            return View(model);
        }
        /*
        [HttpPost]
        public ActionResult AsignarDiasLibres(int empleadoID, List<string> selectedDays)
        {
            // Convierte la lista de días seleccionados en una cadena separada por comas
            string diasSemana = string.Join(",", selectedDays);

            // Llamada para almacenar días libres
            DiasLibresDAL.AsignarDiasLibresEnBaseDatos(empleadoID, diasSemana);

            return RedirectToAction("Success"); // Redirigir a una vista de éxito o cualquier otra acción
        }*/
        private void InsertDiasLibres(int empleadoID, string diasSemana, string tipoDiaLibre)
        {
            using (var connection = new SqlConnection(ConexionString.Conexion))
            {
                using (var command = new SqlCommand("spAsignarDiasLibres", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmpleadoID", empleadoID);
                    command.Parameters.AddWithValue("@DiasSemana", diasSemana);
                    command.Parameters.AddWithValue("@TipoDiaLibre", tipoDiaLibre);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        [HttpPost]
        public IActionResult AsignarDiasLibres(EmpleadoDiasLibresViewModel model)
        {
            Console.WriteLine("ModelState.IsValid 2 " + ModelState.IsValid);
                InsertDiasLibres(model.EmpleadoID, string.Join(",", model.SelectedDiasLibres), model.TipoDiaLibre);
                return RedirectToAction("Index","Home"); // Or some other view
           
            model.Empleados = GetEmpleados();
            model.DiasSemana = new List<string> { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };
            return View(model);
        }


        /*
        public IActionResult AsignarDiasLibres()
        {
            var model = new EmpleadoDiasLibresViewModel
            {
                Empleados = GetEmpleados(),
                DiasSemana = new List<string> { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" }
            };
            return View(model);
        }*/

        private List<EmpleadoModel> GetEmpleados()
        {
            // Replace with actual data fetching logic
            return EmpleadosDAL.ObtenerTodosLosEmpleados(ConexionString.Conexion);
        }


    }
}
