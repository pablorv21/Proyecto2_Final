using System.Data.SqlClient;
using System.Data;

namespace Sistema_Nominas.Util
{
    public class DiasLibresDAL
    {
        public static void AsignarDiasLibresEnBaseDatos(int empleadoID, string diasSemana) 
        {
            using (var connection = new SqlConnection(ConexionString.Conexion))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_AsignarDiasLibres", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@EmpleadoID", empleadoID);
                    command.Parameters.AddWithValue("@DiasSemana", diasSemana);
                     
                    
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
