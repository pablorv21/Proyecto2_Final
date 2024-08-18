using Sistema_Nominas.Models;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace Sistema_Nominas.Util
{
    public class HorariosDAL
    {

        public static List<TurnosModel> BuscarTiposTurno(string terminoBusqueda)
        {
            var tiposTurno = new List<TurnosModel>();

            using (var conn = new SqlConnection(ConexionString.Conexion))
            {
                var command = new SqlCommand("SP_BuscarTiposTurno", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@TerminoBusqueda", terminoBusqueda);

                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tiposTurno.Add(new TurnosModel 
                        {
                            TurnoId = reader.GetInt32(reader.GetOrdinal("TipoTurnoID")),
                            NombreTurnno = reader.GetString(reader.GetOrdinal("Nombre"))
                        });
                    }
                }
            }
            Console.WriteLine("tiposTurno Exception " + tiposTurno);
            return tiposTurno;
        }


        /**
         * Metodo CrearHorario
         * Parametros:: Objecto de tipo Horario
         * Funcionalidad:: Retorna el empleado 
         * */
   
        public static bool CrearHorario(HorariosModels horario)
        {
            bool resultado = false;
            string mensajeSalida;
            try
            {
                using (SqlConnection conn =  new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand cmd= new SqlCommand("sp_InsertarHorario",conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HoraInicio", horario.HoraInicio);
                        cmd.Parameters.AddWithValue("@HoraFin", horario.HoraFin);
                        cmd.Parameters.AddWithValue("@TipoTurnoID", horario.TipoTurnoID);

                        SqlParameter outputParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);
                        cmd.ExecuteNonQuery();
                        mensajeSalida = outputParam.Value.ToString();

                        resultado = mensajeSalida.StartsWith("Nuevo horario creado");
                        return resultado;

                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Expection CrearHorario " + e.Message);
                throw;
            }

        }//

        public static bool ActualizarHorario(HorariosModels horario)
        {
            bool resultado = false;
            string mensajeSalida;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open(); 
                    using (SqlCommand cmd = new SqlCommand("sp_ActualizarHorario", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ID", horario.HorarioID);
                        cmd.Parameters.AddWithValue("@HoraInicio", horario.HoraInicio);
                        cmd.Parameters.AddWithValue("@HoraFin", horario.HoraFin);
                        cmd.Parameters.AddWithValue("@TipoTurnoID", horario.TipoTurnoID);

                        SqlParameter outputParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);
                        cmd.ExecuteNonQuery();
                        mensajeSalida = outputParam.Value.ToString();
                        Console.WriteLine("Horario mensajeSalida " + mensajeSalida);

                        resultado = mensajeSalida.StartsWith("Horario actualizado con éxito.");
                        return resultado;

                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Expection CrearHorario " + e.Message);
                throw;
            }

        }//
        /**
         * Metodo Obtener todos los Horario
         * Parametros:: Objecto de tipo Horario
         * Funcionalidad:: Retorna el empleado 
         * */
        public static List<HorariosModels> ObtenerTodosHorario()
        {
            var Horarios = new List<HorariosModels>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_buscarHorario", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime fecha = reader.GetDateTime(reader.GetOrdinal("Fecha"));
                                HorariosModels horario = new HorariosModels
                                {
                                    HorarioID = reader.GetInt32(reader.GetOrdinal("HorarioID")),
                                    Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                    HoraInicio = reader.GetTimeSpan(reader.GetOrdinal("HoraInicio")),
                                    HoraFin = reader.GetTimeSpan(reader.GetOrdinal("HoraFin")),
                                    TipoTurnoID = reader.GetInt32(reader.GetOrdinal("TipoTurnoID")),
                                    NombreTurno = reader.GetString(reader.GetOrdinal("NombreTurno"))
                                };
                                Horarios.Add(horario);
                            }
                        }
                    }
                }
                return Horarios;

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception ObtenerTodosTurnos " + e.Message);
                throw;
            }
        }
        /**
         * Metodo Obtener todos los Horario por ID
         * Parametros:: Objecto de tipo Horario
         * Funcionalidad:: Retorna el empleado 
         * */
        public static HorariosModels ObtenerTodosHorarioPorId(int horarioId)
        {
            var Horarios = new HorariosModels();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_buscarHorarioPorId", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HararioId", horarioId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime fecha = reader.GetDateTime(reader.GetOrdinal("Fecha"));
                                Horarios = new HorariosModels
                                {
                                    HorarioID = reader.GetInt32(reader.GetOrdinal("HorarioID")),
                                    Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                    HoraInicio = reader.GetTimeSpan(reader.GetOrdinal("HoraInicio")),
                                    HoraFin = reader.GetTimeSpan(reader.GetOrdinal("HoraFin")),
                                    TipoTurnoID = reader.GetInt32(reader.GetOrdinal("TipoTurnoID")),
                                    NombreTurno = reader.GetString(reader.GetOrdinal("NombreTurno"))
                                };
                            }
                        }
                    }
                }
                return Horarios;

            }
            catch (Exception e)
            {
                Console.WriteLine("ObtenerTodosHorarioPorId " + e.Message);
                throw;
            }
        }
        /**
         * Metodo Obtener todos los Horario
         * Parametros:: Objecto de tipo Horario
         * Funcionalidad:: Retorna el empleado 
         * */
        public static Boolean AsignarHorarioUsuario(int horarioId,int empId)
        {
            Boolean resultado = false;
            String mensajeSalida;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_InsertAsignacionTurnos", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmpleadoID", empId);
                        cmd.Parameters.AddWithValue("@HorarioID", horarioId);

                        SqlParameter outputParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);
                        cmd.ExecuteNonQuery();
                        mensajeSalida = outputParam.Value.ToString();
                        Console.WriteLine("mensajeSalida " + mensajeSalida);
                        resultado = mensajeSalida.StartsWith("AsignacionTurnos insertado exitosamente");
                        Console.WriteLine("List resultado " + resultado);
                        return resultado;

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Asignacion Exception "  + e.Message);
                throw;
            }
        }
        public static List<EmpleadoModel> BuscarUsuarios(string query)
        {
            var usuarios = new List<EmpleadoModel>();
            try
            {
                using (var connection = new SqlConnection(ConexionString.Conexion))
                {
                    connection.Open();
                    using (var command = new SqlCommand("sp_BuscarUsuarios", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Query", query);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var usuario = new EmpleadoModel
                                {
                                    EmpleadoID = Convert.ToInt32(reader["EmpleadoID"]),
                                    Nombre = reader["NombreCompleto"].ToString()
                                };
                                usuarios.Add(usuario);
                            }
                        }
                    }
                }
                return usuarios;
            }
            catch (Exception e)
            {
                Console.WriteLine("BuscarUsuarios Exception " + e.Message);
                throw;
            }
  
        }

        public static List<HorariosModels> ObtenerTodosLosHorarios()
        {
            var horarios = new List<HorariosModels>();
            using (var connection = new SqlConnection(ConexionString.Conexion))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_buscarHorario", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var horario = new HorariosModels
                            {
                                HorarioID = reader["HorarioID"] != DBNull.Value ? Convert.ToInt32(reader["HorarioID"]) : 0,
                                NombreTurno = reader["NombreTurno"] != DBNull.Value ? reader["NombreTurno"].ToString() : string.Empty,
                                HoraInicio = reader.GetTimeSpan(reader.GetOrdinal("HoraInicio")),
                                HoraFin = reader.GetTimeSpan(reader.GetOrdinal("HoraFin"))
                            };
                            horarios.Add(horario);
                        }
                    }
                }
            }
            Console.WriteLine("horarios console " + horarios);
            return horarios;
        }

        public static List<CalendarViews> ObtenerCalendarios(int EmpId)
        {
            try
            {
                List<CalendarViews> horarios = new List<CalendarViews>();
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ExtraerHorarios", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmpleadoId", EmpId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var Calendario = new CalendarViews
                                {
                                    LibreId = reader.GetInt32(reader.GetOrdinal("LibreId")),
                                    DiaSemana = reader.GetString(reader.GetOrdinal("DiaSemana")),
                                    TipoDiaLibre = reader.GetString(reader.GetOrdinal("Tipo_DiaLibre")),
                                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                    Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                                    Cedula = reader.GetString(reader.GetOrdinal("Cedula")),                                           
                                    HoraInicio = reader.GetTimeSpan(reader.GetOrdinal("HoraInicio")),
                                    HoraFin = reader.GetTimeSpan(reader.GetOrdinal("HoraFin"))
                                };
                                horarios.Add(Calendario);
                            }
                        }

                    }

                }
                return horarios;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }//
}
