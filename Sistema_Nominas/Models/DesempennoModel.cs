namespace Sistema_Nominas.Models
{
    public class DesempennoModel
    {
        public int EvaluacionID { get; set; }
        public int EmpleadoID { get; set; }
        public int EvaluadorID { get; set; }
        public DateTime FechaEvaluacion { get; set; }
        public int Puntuacion { get; set; }
        public string Comentarios { get; set; }
        public string Objetivos { get; set; }
        public string NombreEvaluador { get; set; }
        public string NombreEvaluado { get; set; }
    }
}
