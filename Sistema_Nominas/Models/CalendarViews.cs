namespace Sistema_Nominas.Models
{
    public class CalendarViews
    {
        public int LibreId { get; set; }
        
        public string DiaSemana { get; set; }
        public string TipoDiaLibre { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public int RolID { get; set; }
        public int HorarioId { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public int TipoTurnoID { get; set; }
        public int EMPID { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraFin { get; set; }
    }
}
