using Sistema_Nominas.Models;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Nominas.Util
{
    public class AdministracionTurnos
    {
        /**
        * Metodo CreacionTurnos
        * Parametros:: Objecto de Tipo turno
        * Funcionalidad:: Crea el turno 
        * */
        public static int CreacionTurnos(TurnosModel turno)
        {
            int TurnoId = 0;
            try
			{
				using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
				{
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_CREAR_TURNOS",conn))
					{
                        Console.WriteLine($"Valor pasado 1: {turno.NombreTurnno}");
                        Console.WriteLine($"Valor pasado 2: {turno.DescriptcionTurnno}");

                        cmd.Parameters.AddWithValue("@Nombre", turno.NombreTurnno);
                        cmd.Parameters.AddWithValue("@Descripcion", turno.DescriptcionTurnno);
                        cmd.CommandType = CommandType.StoredProcedure;
                        /************Slida de datos****************/
                        SqlParameter outputParameter = new SqlParameter();
                        outputParameter.ParameterName = "@NuevoID";
                        outputParameter.SqlDbType = SqlDbType.Int;
                        outputParameter.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outputParameter);

                        Console.WriteLine("@NuevoID = " + cmd.Parameters["@NuevoID"].Value);
                        cmd.ExecuteNonQuery();
                        TurnoId = (int)outputParameter.Value;

                    }

				}
                return TurnoId;

            }
			catch (Exception e)
			{
                Console.WriteLine("CreacionTurnos Exeception " + e.Message);
				throw;
			}
        }
        /**
        * Metodo ObtenerTodosTurnos
        * Parametros:: Objecto de Tipo turno
        * Funcionalidad:: OBTIENE LOS TIPOS DE TURNOS
        * */
        public static List<TurnosModel> ObtenerTodosTurnos()
        {
            var Turnos = new List<TurnosModel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_OBTENERTURNOS",conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TurnosModel turnos = new TurnosModel
                                {
                                    TurnoId = reader.GetInt32(reader.GetOrdinal("TipoTurnoID")),
                                    NombreTurnno = reader.GetString(reader.GetOrdinal("Nombre")),
                                    DescriptcionTurnno = reader.GetString(reader.GetOrdinal("Descripcion")),
                                };
                                Turnos.Add(turnos);
                            }                          
                        }
                    }
                }
                return Turnos;

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception ObtenerTodosTurnos " + e.Message);
                throw;
            }
        }
        /**
        * Metodo ActualizarTipoTurno
        * Parametros:: Objecto de Tipo turno
        * Funcionalidad:: Actualiza el Turno
        * Return :: devuelve un boolean
        * */
        public static bool EditarTipoTurno(TurnosModel tipoModel)
        {
            bool resultado = false;
            string mensajeSalida;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_TIPOTURNO", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TURNOID", tipoModel.TurnoId);
                        cmd.Parameters.AddWithValue("@NuevoNombre", tipoModel.NombreTurnno);
                        cmd.Parameters.AddWithValue("@NuevaDescripcion", tipoModel.DescriptcionTurnno);
                       
                        SqlParameter outputParam = new SqlParameter("@MensajeSalida", SqlDbType.NVarChar, 1000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);
                        cmd.ExecuteNonQuery();
                        mensajeSalida = outputParam.Value.ToString();

                        resultado = mensajeSalida.StartsWith("Actualización exitosa");                        
                        return resultado;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Editar EditarTipoTurno " + e.Message);
                throw;
            }
        }
        /**
        * Metodo ObtenerTurnoID
        * Parametros:: Objecto de Tipo turno
        * Funcionalidad:: Actualiza el Turno
        * Return :: devuelve un boolean
        * */
        public static TurnosModel ObtenerTurnoID(int turnId)
        {
            var Turnos = new TurnosModel();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_OBTENERTURNOS_ID", conn))
                    {

                        cmd.Parameters.AddWithValue("@TIPOTURNOID", turnId);
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Turnos = new TurnosModel
                                {
                                    TurnoId = reader.GetInt32(reader.GetOrdinal("TipoTurnoID")),
                                    NombreTurnno = reader.GetString(reader.GetOrdinal("Nombre")),
                                    DescriptcionTurnno = reader.GetString(reader.GetOrdinal("Descripcion")),
                                };
                               
                            }
                        }
                    }
                }
                return Turnos;

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception ObtenerTodosTurnos " + e.Message);
                throw;
            }
        }

    }
}
