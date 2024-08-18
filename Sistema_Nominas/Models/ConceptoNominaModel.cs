namespace Sistema_Nominas.Models
{
    public class ConceptoNominaModel
    {
        public int conceptoId { get; set; }
        public String Nombre { get; set; }
        public String TipoNomina { get; set; }
        public Decimal Porcentaje { get; set; }
        public Decimal Umbral { get; set; }
    }
}
