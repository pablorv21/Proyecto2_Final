using Microsoft.AspNetCore.Mvc;
using Sistema_Nominas.Models;
using Sistema_Nominas.Util;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sistema_Nominas.Controllers
{
    public class DesempenoController : Controller
    {
        public IActionResult Nuevo()
        {
            return View();
        }
        public IActionResult MenuPrincipal()
        {
            return View();
        }
        public IActionResult Editar(int id)
        {
            DesempennoModel model = DesempennoDAL.ObtenerEvaluacionPorId(id);
            return View(model);
        }

        public IActionResult ObservarEvalaciones(string Nombre )  
        {
            var evaluaciones = new List<DesempennoModel>();
            if (string.IsNullOrEmpty(Nombre))
            {
                evaluaciones = DesempennoDAL.ObtenerTodasLasEvaluaciones();
            }
            else
            {
                evaluaciones = DesempennoDAL.ObtenerEvaliaciones(Nombre);
            }
            return View(evaluaciones);
        }

        [HttpPost]
        public JsonResult CrearDesempeno(DesempennoModel model)
        {
            
            try
            {
                int resultado = DesempennoDAL.AgregarDesempenno(model);
                if (resultado>0)
                {
                    return Json(new { success = true, message = "Registro creado exitosamente" });
                }
                else
                {
                    return Json(new { success = false, message = "Registro creado exitosamente" });
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
                throw;
            }

        }

        [HttpPost]
        public JsonResult ActualizarEvaluacion(DesempennoModel model)
        {

            try
            {
                bool resultado = DesempennoDAL.ActualizarEvaluacion(model);
                if (resultado)
                {
                    return Json(new { success = true, message = "Registro creado exitosamente" });
                }
                else
                {
                    return Json(new { success = false, message = "Registro creado exitosamente" });
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
                throw;
            }

        }

    }/**/
}
