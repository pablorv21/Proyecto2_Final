using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sistema_Nominas.Models
{
    public class HorarioModelView
    {
        public HorariosModels Horarios { get; set; }
        public List<SelectListItem> Turnos { get; set; }
    }
}
