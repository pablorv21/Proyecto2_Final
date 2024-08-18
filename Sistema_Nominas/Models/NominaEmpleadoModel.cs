namespace Sistema_Nominas.Models
{
    public class NominaEmpleadoModel
    {
        public int NominaID { get; set; } 

        public int EmpleadoID { get; set; } 

        public DateTime PeriodoInicio { get; set; } 

        public DateTime PeriodoFin { get; set; } 

        public DateTime FechaGeneracion { get; set; } 

        public decimal SalarioBase { get; set; } 

        public decimal TotalIngresos { get; set; } 

        public decimal TotalDeducciones { get; set; } 

        public decimal TotalNeto { get; set; }
        public List<DetalleNominaViewModel> DetalleNomina { get; set; }

    }
}
