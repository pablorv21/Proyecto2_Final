namespace Sistema_Nominas.Models
{
    public class NominaViewModel
    {
		public int NominaID { get; set; }
		public string NombreEmpleado { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFin { get; set; }
        public decimal SalarioBase { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalDeducciones { get; set; }
        public decimal TotalNeto { get; set; }
        public List<DetallesNominaViewModel> Detalles { get; set; }
    }
    public class DetallesNominaViewModel
    {
        public string NombreConcepto { get; set; }
        public string TipoConcepto { get; set; }
        public decimal Monto { get; set; }
    }
}
