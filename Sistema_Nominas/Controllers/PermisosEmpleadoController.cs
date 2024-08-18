using Microsoft.AspNetCore.Mvc;
using Sistema_Nominas.Models;
using Sistema_Nominas.Util;

namespace Sistema_Nominas.Controllers
{
    public class PermisosEmpleadoController : Controller
    {
        public IActionResult Permisos()
        {
            return View();
        }

        public ActionResult AnadirPermisos(int empleadoID)
        {
            List<PermisoModel> permisos = PermisosDAL.ObtenerPermisos();
            ViewBag.EmpleadoID = empleadoID;
            return View(permisos);
        }

        [HttpPost]
        public IActionResult GuardarPermisos(int empleadoID, List<PermisoModel> permisos)
        {
            var permisosSeleccionados = permisos
                .Where(p => p.IsSelected)
                .Select(p => p.PermisoId)
                .ToList();

            if (permisosSeleccionados.Count == 0)
            {
                ModelState.AddModelError("", "Debe seleccionar al menos un permiso.");
                return View(permisos); // Volver a la vista si no hay permisos seleccionados
            }

            // Llamar al servicio para asignar los permisos
            string mensajeResultado = PermisosDAL.AsignarPermisosEmpleado(empleadoID, permisosSeleccionados);

            // Mostrar mensaje en la vista
            ViewBag.Mensaje = mensajeResultado;

            return RedirectToAction("EmpleadoPerfil","Empleados");
        }
    }
}
