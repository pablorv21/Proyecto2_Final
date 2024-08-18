using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Nominas.Models;
using Sistema_Nominas.Util;

namespace Sistema_Nominas.Controllers
{
    public class HorariosController : Controller
    {
        // GET: HorariosController
        public ActionResult NuevoHorario()
        {
            var model = new HorarioModelView
            {
                Horarios = new HorariosModels(),
                Turnos = ObtenerTurnos()
            };
            Console.WriteLine(model.Turnos);
            return View(model);
        }
        /**
         *Metodo: Obtenertodos los horarios
         *Parametro: N/a
         *Funcionalidad: Retorna los Horario
         **/
        public ActionResult VerHorarios()
        {
            List<HorariosModels> Horarios = HorariosDAL.ObtenerTodosHorario();
            return View(Horarios);
        }
        public ActionResult AsignarHorario()
        {
            return View();
        }

        public IActionResult ObtenerHorarios(int empleadoId)
        {
            List<CalendarViews> caledars = HorariosDAL.ObtenerCalendarios(1);
            return View(caledars);
        }

        /**
         *Metodo: GetRoles
         *Parametro: N/a
         *Funcionalidad: Metodo retorna los roles de la BD
         **/
        private List<SelectListItem> ObtenerTurnos()
        {
            var turnos = AdministracionTurnos.ObtenerTodosTurnos();
            Console.WriteLine(turnos);
            return turnos.Select(r => new SelectListItem
            {
                Value = r.TurnoId.ToString(),
                Text = r.NombreTurnno
            }).ToList();
        }
        /**
         *Metodo: GetRoles
         *Parametro: N/a
         *Funcionalidad: Metodo retorna los roles de la BD
         **/
        public ActionResult ActualizarHorario(int horarios)
        {
            var model = HorariosDAL.ObtenerTodosHorarioPorId(horarios);
            return View(model);
        }

        /**
         *Metodo: CrearHorario
         *Parametro:Objecto horarioModels
         *Funcionalidad: Metodo retorna los roles de la BD
         **/
        [HttpPost]
        public JsonResult CrearHorario(HorariosModels horario)
        {
            Console.WriteLine("CrearHorario");
            Boolean resultado = false;
            try
            {
                Console.WriteLine("Ingreso al CrearHorario");
                resultado = HorariosDAL.CrearHorario(horario);
                if (resultado)
                {
                    return Json(new { success = true, message = "Horario creado correctamente" });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudo crear el horario" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /**
         *Metodo: ActualizarHorario
         *Parametro: HorariosModels
         *Funcionalidad: Actualizar
         **/
        [HttpPost]
        public JsonResult ActualizarHorario(HorariosModels horario)
        {
            Console.WriteLine("ActualizarHorario");
            Boolean resultado = false;
            try
            {
                Console.WriteLine("Ingreso al ActualizarHorario");
                resultado = HorariosDAL.ActualizarHorario(horario);
                if (resultado)
                {
                    return Json(new { success = true, message = "Horario actualizado correctamente" });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudo actualizar el horario" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        /**
         *Metodo: ActualizarHorario
         *Parametro: HorariosModels
         *Funcionalidad: Actualizar
         **/
        [HttpPost]
        public JsonResult AsignarHorarioUsuario(int horarioId,int empId)
        {
            Console.WriteLine("ActualizarHorario");
            Boolean resultado = false;
            try
            {
                Console.WriteLine("Ingreso al asignar");
                resultado = HorariosDAL.AsignarHorarioUsuario(horarioId, empId);
                Console.WriteLine("Error en resultado" + horarioId +"<><>"+ empId);
                if (resultado)
                {
                    return Json(new { success = true, message = "Horario Asignado correctamente" });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudo asignar el horario" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en asignar" + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        public JsonResult BuscarUsuarios(string query)
        {
            var usuarios = HorariosDAL.BuscarUsuarios(query);
            var resultado = usuarios.Select(u => new { id = u.EmpleadoID, nombre = u.Nombre });
            return Json(resultado);
        }

        // Método para obtener todos los horarios
        [HttpGet]
        public JsonResult ObtenerTodosLosHorarios()
        {
            try
            {
                var horarios = HorariosDAL.ObtenerTodosLosHorarios();
                var resultado = horarios.Select(h => new { id = h.HorarioID, descripcion = h.NombreTurno,HoraInicio =h.HoraInicio,HoraFin=h.HoraFin });
                Console.WriteLine("var resultado " + resultado);
                return Json(resultado);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al obtener horarios: " + ex.Message });
            }
        }
    }
}
