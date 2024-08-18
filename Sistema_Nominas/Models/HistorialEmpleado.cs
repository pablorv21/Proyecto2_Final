using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Nominas.Models
{
    public class HistorialEmpleado
    {
        public int HistorialID { get; set; }
        public int EmpleadoID { get; set; }
        public int RolID { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
        public string nombreRole { get; set; }
    }
}
