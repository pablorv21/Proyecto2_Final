using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Nominas.Interfaces;
using Sistema_Nominas.Models;
using Sistema_Nominas.Util;
using System.Text;

namespace Sistema_Nominas.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly IViewRenderService _viewRenderService;
        /*Contructor Para Utilizar las interfaces*/
        public EmpleadosController(IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
        }

        /**
         *Metodo: Empleados
         *Parametro: N/a
         *Funcionalidad: Metodo para hacer redirect a la vista de empleados
         *Return :: Una lista de Empleados
         **/
        public IActionResult Empleados()
        {
            List<EmpleadoModel> EmpleadosLST = EmpleadosDAL.ObtenerTodosLosEmpleados(ConexionString.Conexion);
            return View(EmpleadosLST);
        }
        /**
         *Metodo: ActualizarEmpleado
         *Parametro: N/a
         *Funcionalidad: Metodo para hacer redirect a la vista de informacion del Empleado para actualizar
         *Return :: View
         **/
        public IActionResult ActualizarEmpleado(int empId)
        {
            Console.WriteLine("empId " + empId);
            var empleado = EmpleadosDAL.ObtenerEmpleadoPorID(empId);
            if (empleado == null)
            {
                Console.WriteLine("empleado empId " + empleado);
            }

            var model = new EmpleadoViewModel
            {
                Empleado = empleado,
                Roles = GetRoles()
            };

            return View(model);

        }
        /*
        public IActionResult ObtnerLosRoleEmpleado(int empId)
        {
            List<HistorialEmpleado> RolesList = HistorialRolesDAL.ObtenerHistorialEmpleado(empId);
            return View(RolesList);

        }*/

        public IActionResult ActualizarRole(int empId) 
        {
            Console.WriteLine("empId " + empId);
            var empleado = EmpleadosDAL.ObtenerEmpleadoPorID(empId);
            if (empleado == null)
            {
                Console.WriteLine("empleado empId " + empleado);
            }

            var model = new EmpleadoViewModel
            {
                Empleado = empleado,
                Roles = GetRoles()
            };

            return View(model);
        }
        /**
         *Metodo: PerfilEmpleado
         *Parametro: ID del empleado
         *Funcionalidad: return a la vista para analizar el peril del usuario. 
         *Return :: View
         **/
        public IActionResult EmpleadoPerfil(int empId)
        {
            Console.WriteLine("empId " + empId);
            var empleado = EmpleadosDAL.ObtenerEmpleadoPorID(empId);
            if (empleado == null)
            {
                Console.WriteLine("empleado empId " + empleado);
            }

            var model = new EmpleadoViewModel
            {
                Empleado = empleado,
                Roles = GetRoles(),
                HistorialRoles = HistorialRolesDAL.ObtenerHistorialEmpleado(empId)
            };

            return View(model);
        }
        /**
         *Metodo: EditarEmpleado
         *Parametro: EmpleadoViewModel
         *Funcionalidad: Realiza el redirect para la pagina correcta
         *Return :: View
         **/
        public JsonResult EditarEmpleado(EmpleadoViewModel model)
        {
            Console.WriteLine("EditarEmpleado" + model);
            Boolean resultado = false;
            try
            {
                resultado = EmpleadosDAL.EditarEmpleado(model.Empleado);
                if (resultado)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            return Json(new { success = resultado });
        }

        /**
         *Metodo: Empleado
         *Parametro: N/a
         *Funcionalidad: Metodo para hacer redirect a la vista de informacion del Empleado
         *Return :: View
         **/
        public IActionResult InformacionEmpleado()
        {          
            return View("InformacionEmpleado");
        }
        /**
         *Metodo: CreacionEmpleados
         *Parametro: N/a
         *Funcionalidad: Metodo para cargar la informacion y crear Empleados
         **/
        public IActionResult CreacionEmpleados()
        {
            var model = new EmpleadoViewModel
            {
                Empleado = new EmpleadoModel(),
                Roles = GetRoles()
            };
            Console.WriteLine(model.Roles);
            return View(model);
            //return View();
        }
        /**
         *Metodo: CreacionEmpleados
         *Parametro: N/a
         *Funcionalidad: Metodo Carga los Roles en el Form.
         **/
        [HttpPost]
        public ActionResult CreacionEmpleados(EmpleadoViewModel model)
        {
            try
            {
                Console.WriteLine("Role " + model.Empleado.RolID);
                Console.WriteLine("State " + ModelState.IsValid);
                Console.WriteLine("State ");

                int emp = EmpleadosDAL.CrearNuevoEmpleado(model.Empleado);
                if (emp > 0)
                {
                    return RedirectToAction("Empleados");
                }
                else
                {
                    model.Roles = GetRoles();
                    return View(model);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Crear Empleado " + e.Message);
                throw;
            }

        }
        /**
         * Metodo: ActualizarEmpleado
         * Parametro: Objeto e tipo EmpleadoViewModel
         * Funcionalidad: REaliza una actualizacion
         **/

        [HttpPost]
        public ActionResult ActualizarEmpleado(EmpleadoViewModel model)
        {
            try
            {
                Console.WriteLine("Role " + model.Empleado.RolID);
                Console.WriteLine("State " + ModelState.IsValid);
                Console.WriteLine("State ");

                bool emp = EmpleadosDAL.EditarEmpleado(model.Empleado);
                Console.WriteLine("emp " + emp);
                if (emp)
                {
                    TempData["SuccessMessage"] = "Empleado actualizado correctamente.";
                    return RedirectToAction("Empleados", "Empleados");
                }
                else
                {
                    // Si hay un error, volver a cargar los roles
                    model.Roles = GetRoles();
                    return View(model);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Crear Empleado " + e.Message);
                throw;
            }

        }
        /**
         * Metodo: ActualizarNuevoRoleJS
         * Parametro: Objeto e tipo EmpleadoViewModel
         * Funcionalidad: REaliza una actualizacion
         **/
        [HttpPost]
        public JsonResult ActualizarNuevoRoleJS(int empId, int roleId)
        {
            Console.WriteLine("ActualizarNuevoRoleJS");
            Boolean resultado = false;
            try
            {
                Console.WriteLine("Ingreso al ac");
                resultado = HistorialRolesDAL.NuevoRoleEmpleado(empId, roleId);
                if (resultado)
                {
                    return Json(new { success = true, message = "Role actualizado correctamente" });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudo actualizar el role" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /**
         *Metodo: GetRoles
         *Parametro: N/a
         *Funcionalidad: Metodo retorna los roles de la BD
         **/
        private List<SelectListItem> GetRoles()
        {
            var roles = EmpleadosDAL.ObtenerLosRoles();
            Console.WriteLine(roles);
            return roles.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.NombreRol
            }).ToList();
        }




    }//Fin de la clase
}//Fin del name space
