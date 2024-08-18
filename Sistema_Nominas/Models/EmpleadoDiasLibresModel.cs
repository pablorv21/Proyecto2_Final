using System.ComponentModel.DataAnnotations;

namespace Sistema_Nominas.Models
{
    public class EmpleadoDiasLibresModel
    {
        public int EmpleadoID { get; set; }

        [Display(Name = "Días Libres")]
        public List<string> SelectedDiasLibres { get; set; } = new List<string>();
        public List<EmpleadoModel> Empleados { get; set; }
        public List<string> DiasSemana { get; set; }

        [Display(Name = "Tipo de Día Libre")]
        public string TipoDiaLibre { get; set; }
    }
}
