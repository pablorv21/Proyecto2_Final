using System.ComponentModel.DataAnnotations;

namespace Sistema_Nominas.Models
{
    public class SalarioEmpleadoModel
    {
    
        public int SalarioID { get; set; }
        public int EmpleadoID { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El salario base debe ser un valor positivo.")]
        public decimal SalarioBase { get; set; }
        public string TipoSalario { get; set; } 
        public String Nombre_Empleado { get; set; }

        public String cedula { get; set; }
    }
}
