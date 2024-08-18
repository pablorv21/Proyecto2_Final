using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Sistema_Nominas.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sistema_Nominas.Util
{
    public class DesempennoDAL
    {
        public static int AgregarDesempenno(DesempennoModel model)
        {
            int evaluacionID = 0;
            string mensaje = string.Empty;

            try
            {
                Console.WriteLine("Desep@");
				using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
				{
					conn.Open();
					using (SqlCommand cmd = new SqlCommand("SP_InsertarEvaluacionDesempeno",conn))
					{
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmpleadoID", model.EmpleadoID);
                        cmd.Parameters.AddWithValue("@EvaluadorID",model.EvaluadorID);
                        cmd.Parameters.AddWithValue("@FechaEvaluacion", model.FechaEvaluacion);
                        cmd.Parameters.AddWithValue("@Puntuacion", model.Puntuacion);
                        cmd.Parameters.AddWithValue("@Comentarios", (object)model.Comentarios ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Objetivos", (object)model.Objetivos ?? DBNull.Value);

                        SqlParameter evaluacionIdParam = new SqlParameter("@EvaluacionID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(evaluacionIdParam);

                        SqlParameter mensajeParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(mensajeParam);
                        cmd.ExecuteNonQuery();
                        //Console.WriteLine("error en el sitema " + (int)evaluacionIdParam.Value);
                        mensaje = mensajeParam.Value.ToString();
                        Console.WriteLine("error en el sitema " + mensaje);

                        evaluacionID = (int)evaluacionIdParam.Value;
                       

                    }

                }
                return evaluacionID;

			}
			catch (Exception e)
			{
                Console.WriteLine("Error Desempleño " + e.Message);
				throw;
			}

        }

        public static List<DesempennoModel> ObtenerEvaliaciones(string Nombre)
        {
            var ListaEvaluaciones = new List<DesempennoModel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    
                    using (SqlCommand cmd = new SqlCommand("SP_ObtenerEvaluacionesDesempeno", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nombre", Nombre);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read()) 
                            {
                                var evaluaciones = new DesempennoModel
                                {
                                    NombreEvaluado = reader.GetString(reader.GetOrdinal("Nombre")),
                                    //NombreEvaluador = reader.GetString(reader.GetOrdinal("NombreEvaluador")),
                                    FechaEvaluacion = reader.GetDateTime(reader.GetOrdinal("FechaEvaluacion")),
                                    Puntuacion = reader.GetInt32(reader.GetOrdinal("Puntuacion")),
                                    Comentarios = reader.GetString(reader.GetOrdinal("Comentarios")),
                                    Objetivos = reader.GetString(reader.GetOrdinal("Objetivos")),
                                };
                                ListaEvaluaciones.Add(evaluaciones);
                            }

                        }

                    }
                }
                return ListaEvaluaciones;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<DesempennoModel> ObtenerTodasLasEvaluaciones()
        {
            var evaluaciones = new List<DesempennoModel>();

            using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_ObtenerTodasEvaluacionesDesempeno", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var desempenno = new DesempennoModel
                            {
                                EvaluacionID = Convert.ToInt32(reader["EvaluacionID"]),
                                NombreEvaluado = reader["NombreEvaluado"].ToString(),
                                NombreEvaluador = reader["NombreEvaluador"].ToString(),
                                FechaEvaluacion = Convert.ToDateTime(reader["FechaEvaluacion"]),
                                Puntuacion = Convert.ToInt32(reader["Puntuacion"])
                            };
                            evaluaciones.Add(desempenno);
                        }
                    }
                }
            }

            return evaluaciones;
        }

        public static bool ActualizarEvaluacion(DesempennoModel model)
        {
            bool resultado = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    string query = @"UPDATE EvaluacionesDesempeño 
                                   SET EmpleadoID = @EmpleadoID,
                                   EvaluadorID = @EvaluadorID,
                                   FechaEvaluacion = @FechaEvaluacion,
                                   Puntuacion = @Puntuacion,
                                   Comentarios = @Comentarios,
                                   Objetivos = @Objetivos
                                   WHERE EvaluacionID = @EvaluacionID";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EvaluacionID", model.EvaluacionID);
                        cmd.Parameters.AddWithValue("@EmpleadoID", model.EmpleadoID);
                        cmd.Parameters.AddWithValue("@EvaluadorID", model.EvaluadorID);
                        cmd.Parameters.AddWithValue("@FechaEvaluacion", model.FechaEvaluacion);
                        cmd.Parameters.AddWithValue("@Puntuacion", model.Puntuacion);
                        cmd.Parameters.AddWithValue("@Comentarios", (object)model.Comentarios ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Objetivos", (object)model.Objetivos ?? DBNull.Value);

                        int filasAfectadas = cmd.ExecuteNonQuery();
                        resultado = filasAfectadas > 0;
                    }

                }
                return resultado;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        public static DesempennoModel ObtenerEvaluacionPorId(int id)
        {
            var desempenno = new DesempennoModel();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ObtenerEvaluacion", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read()) 
                            {
                                desempenno = new DesempennoModel
                                {
                                    EvaluacionID = reader.GetInt32(reader.GetOrdinal("EvaluacionID")),
                                    NombreEvaluado = reader.GetString(reader.GetOrdinal("Nombre")),
                                    EmpleadoID = reader.GetInt32(reader.GetOrdinal("Expr1")),
                                    EvaluadorID = reader.GetInt32(reader.GetOrdinal("EvaluadorID")),
                                    //NombreEvaluador = reader.GetString(reader.GetOrdinal("NombreEvaluador")),
                                    FechaEvaluacion = reader.GetDateTime(reader.GetOrdinal("FechaEvaluacion")),
                                    Puntuacion = reader.GetInt32(reader.GetOrdinal("Puntuacion")),
                                    Comentarios = reader.GetString(reader.GetOrdinal("Comentarios")),
                                    Objetivos = reader.GetString(reader.GetOrdinal("Objetivos")),
                                };
                            }
                        }
                    }

                }
                return desempenno;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }


    }/**/
}
