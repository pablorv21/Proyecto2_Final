namespace Sistema_Nominas.Models
{
    public class DetalleNominaViewModel
    {
        public int DetalleID { get; set; } 
        public int NominaID { get; set; } 
        public int ConceptoID { get; set; } 
        public string NombreConcepto { get; set; } 
        public decimal Monto { get; set; }
    }
}
