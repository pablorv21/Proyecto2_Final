using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Nominas.Models
{
    public class ConceptoDeducionesSalariales
    {
        public int ConceptoSalarioID { get; set; }       
        public int SalarioID { get; set; }       
        public int ConceptoID { get; set; }
    }
}
