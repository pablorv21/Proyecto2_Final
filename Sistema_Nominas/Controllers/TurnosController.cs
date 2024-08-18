using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Nominas.Models;
using Sistema_Nominas.Util;

namespace Sistema_Nominas.Controllers
{
    public class TurnosController : Controller
    {
        // GET: TurnosController
        public ActionResult CreacionTurnos()
        {
            return View();
        }
        public ActionResult MenuPrincipal()
        {
            return View();
        }
        public ActionResult ListadoTurnos()
        {
            List<TurnosModel> model = AdministracionTurnos.ObtenerTodosTurnos();
            return View(model);
        }
        public ActionResult ActualizarTurno(int turnoId)
        {
            TurnosModel model = AdministracionTurnos.ObtenerTurnoID(turnoId);
            return View(model);
        }

        [HttpPost]
        public JsonResult CreacionTurnos(TurnosModel turno)
        {
            try
            {
                int NuevoTurno = AdministracionTurnos.CreacionTurnos(turno);
                if (NuevoTurno > 0)
                {
                    return Json(new { success = true, message = "Nuevo Turno Creado" });

                }
                else
                {
                    return Json(new { success = false, message = "No se Logro Crear Turno Creado" });
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("CreacionTurnos Exception" + e.Message);
                return Json(new { success = false, message = e.Message });
            }

        }

        [HttpPost]
        public JsonResult ActualizarTipoTurnoJS(TurnosModel turnoModel )
        {
            Console.WriteLine("ActualizarNuevoRoleJS");
            Boolean resultado = false;
            try
            {
                Console.WriteLine("Ingreso al ac");
                resultado = AdministracionTurnos.EditarTipoTurno(turnoModel);
                if (resultado)
                {
                    return Json(new { success = true, message = "Turno actualizado correctamente" });
                }
                else 
                {
                    return Json(new { success = false, message = "No se pudo actualizar el turno" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }






    }
}
