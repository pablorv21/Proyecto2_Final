using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Nominas.Models;
using Sistema_Nominas.Util;

namespace Sistema_Nominas.Controllers
{
    public class Salarios : Controller
    {
        public ActionResult MenuPrincipal()
        {
            return View();
        }
        public IActionResult ObtenerConceptos()
        {
            var conceptos = SalariosEmpleadosDAL.GetConceptosNomina();
            return Json(conceptos);
        }
        public ActionResult AgregarSalario()
        {
            var conceptos = SalariosEmpleadosDAL.GetConceptosNomina();           
            ViewBag.Conceptos = conceptos;
            //ViewBag.Conceptos = new SelectList(SalariosEmpleadosDAL.GetConceptosNomina(), "ConceptoID", "Nombre");
            return View();
        }
        public ActionResult VerSalarios()
        {
            List<SalarioEmpleadoModel> salariosLst = SalariosEmpleadosDAL.ObtenerSalarios();
            return View(salariosLst);
        }

        public IActionResult ReportesSalarios()
        {
            List<SalarioEmpleadoModel> salarios = SalariosEmpleadosDAL.ObtenerSalariosReporte();
            return View(salarios);
        }

        [HttpPost]
        public JsonResult CrearSalario_Empleado(SalarioEmpleadoModel model)
        {
            Boolean resultado;
            try
            {
                int salarioId = SalariosEmpleadosDAL.AgregarSalarioEmpleado(model);
                if (salarioId > 0)
                {
                    return Json(new { success = true, message = "Salario Agregado correctamente", salarioId = salarioId });
                }
                else
                {
                    return Json(new { success = false, message = "Error en el ingreso de datos" });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Slario tuvo error " + e.Message);
                throw;
            }
        }/**/

        [HttpPost]
        public IActionResult AsignarConceptos(int salarioID, List<int> ConceptosSeleccionados)
        {
            Console.WriteLine("Entro al AsignarConcepto");

            foreach (var conceptoID in ConceptosSeleccionados)
            {
                Console.WriteLine("Entro al AsignarConcepto 2 " + conceptoID);
                SalariosEmpleadosDAL.GuardarConceptoSalario(salarioID, conceptoID); 
            }

            return Json(new { success = true });
        }




    }
}
