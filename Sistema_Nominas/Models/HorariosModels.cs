using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sistema_Nominas.Models
{
    public class HorariosModels
    {
        public int HorarioID { get; set; }
        public int EmpleadoID { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int TipoTurnoID { get; set; }
        public string NombreTurno { get; set; }


    }
}
