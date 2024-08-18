using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sistema_Nominas.Models
{
    public class EmpleadoViewModel
    {
        public EmpleadoModel Empleado { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public List<HistorialEmpleado> HistorialRoles { get; set; }
    }
}
