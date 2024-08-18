using Sistema_Nominas.Models;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Nominas.Util
{
    public class HistorialRolesDAL
    {
        /**
         * Metodo: NuevoRoleEmpleado
         * Parametro: Objeto e tipo EmpleadoViewModel
         * Funcionalidad: REaliza una actualizacion
         **/
        public static bool NuevoRoleEmpleado(int empId, int roleId)
        {           
			try
			{
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_ACTUALIZARROLESEMP", conn))
                    {
                        Console.WriteLine("empId" + empId);
                        Console.WriteLine("roleId " + roleId);

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmpleadoID", empId);
                        cmd.Parameters.AddWithValue("@NuevoRolID", roleId); 
                        
                        int numeroAfectados = cmd.ExecuteNonQuery();
                        Console.WriteLine("numero Afectados " + numeroAfectados);

                        if (numeroAfectados > 0 || numeroAfectados == -1)
                        {
                            return true;
                        }
                       
                    }
                }          
            }
			catch (Exception e)
			{
                Console.WriteLine("Exception " + e.Message);
                return false;
                throw;
			}
            return false;
        }
        /**
         * Metodo: NuevoRoleEmpleado
         * Parametro: Objeto e tipo EmpleadoViewModel
         * Funcionalidad: REaliza una actualizacion
         **/
        public static List<HistorialEmpleado> ObtenerHistorialEmpleado(int empId)
        {
            var historialList = new List<HistorialEmpleado>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_HISTORIALDEROLES_EMP", conn))
                    {
                       
                        cmd.Parameters.AddWithValue("@EmpleadoID", empId);
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio"));

                                DateTime? FechaTerminacion2 = reader.IsDBNull(reader.GetOrdinal("FechaFin"))
                                                              ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFin"));
                                HistorialEmpleado historialEmpleado = new HistorialEmpleado
                                {     
                                    nombreRole  =  reader.GetString(reader.GetOrdinal("NombreRol")),
                                    EmpleadoID  =  reader.GetInt32(reader.GetOrdinal("EmpleadoID")),
                                    FechaInicio =  DateOnly.FromDateTime(FechaInicio),
                                    FechaFin    = FechaTerminacion2.HasValue ? DateOnly.FromDateTime(FechaTerminacion2.Value) : null,

                                };
                                historialList.Add(historialEmpleado);

                            }
                        }
                    }

                }
                return historialList;

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception ObtenerHistorialEmpleado " + e.Message);

                throw;
            }
        }




    }
}
