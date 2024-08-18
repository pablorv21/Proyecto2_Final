using System.Data;
using System.Data.SqlClient;
using Sistema_Nominas.Models;

namespace Sistema_Nominas.Util
{
    public class PermisosDAL
    {
        public static List<PermisoModel> ObtenerPermisos()
        {
            var PermisosList = new List<PermisoModel>();
            try
            {
                using (SqlConnection conn= new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_OBTENERPERMISOS",conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var PermisosM = new PermisoModel
                                {
                                    PermisoId = reader.GetInt32(reader.GetOrdinal("PermisoID")),
                                    PermisoName = reader.GetString(reader.GetOrdinal("NombrePermiso")),
                                    Descripcion = reader.GetString(reader.GetOrdinal("Descripcion"))
                                };
                                PermisosList.Add(PermisosM);
                            }

                        }

                    }

                }
                return PermisosList;

            }
            catch (Exception)
            {

                throw;
            }
        }/**/
        public static string AsignarPermisosEmpleado(int empleadoID, List<int> permisosSeleccionados)
        {
            Console.WriteLine("Permisos Usuario " + empleadoID);
            string mensajeResultado;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("SP_InsertarPermisosEmpleado", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EmpleadoID", empleadoID);
                        SqlParameter permisosParam = new SqlParameter
                        {
                            ParameterName = "@PermisosSeleccionados",
                            SqlDbType = SqlDbType.Structured,
                            TypeName = "dbo.PermisoIDList", 
                            Value = CreatePermisosDataTable(permisosSeleccionados)
                        };
                        command.Parameters.Add(permisosParam);
                        SqlParameter mensajeParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 250)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(mensajeParam);
                        command.ExecuteNonQuery();

                        // Recuperar el mensaje de salida
                        mensajeResultado = (string)command.Parameters["@Mensaje"].Value;
                    }
                }
                return mensajeResultado;
            }
           
            catch (Exception)
            {

                throw;
            }

        }

        private static DataTable CreatePermisosDataTable(List<int> permisosSeleccionados)
        {
            DataTable permisosTable = new DataTable();
            permisosTable.Columns.Add("PermisoID", typeof(int));

            foreach (var permisoID in permisosSeleccionados)
            {
                permisosTable.Rows.Add(permisoID);
            }

            return permisosTable;
        }

    }
}
