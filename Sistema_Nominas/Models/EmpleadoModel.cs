using System.Security.Cryptography.Xml;

namespace Sistema_Nominas.Models
{
    public class EmpleadoModel
    {
        public int EmpleadoID { get; set; }
        public String Nombre { get; set; }
        public String Apellido { get; set; }        
        public String CorreoElectronico { get; set; }
        public String Telefono { get; set; }
        public String Direccion { get; set; }
        public DateOnly FechaContratacion { get; set; }
        public DateOnly? FechaTerminacion { get; set; }
        public int RolID { get; set; }
        public String Cedula { get; set; }
        public String NombreRole { get; set; }
        public String DescriptionRole { get; set; }
  
    }
}
