using Sistema_Nominas.Models;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using Sistema_Nominas.Controllers;

namespace Sistema_Nominas.Util
{
    public class SalariosEmpleadosDAL
    {

        public static int AgregarSalarioEmpleado(SalarioEmpleadoModel model)
        {
            int salarioId = 0;
            string message = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("sp_InsertarSalarioEmpleado", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@EmpleadoID", model.EmpleadoID);
                        command.Parameters.AddWithValue("@FechaInicio", model.FechaInicio);
                        command.Parameters.AddWithValue("@FechaFin", (object)model.FechaFin ?? DBNull.Value);
                        command.Parameters.AddWithValue("@SalarioBase", model.SalarioBase);
                        command.Parameters.AddWithValue("@TipoSalario", model.TipoSalario);

                        SqlParameter mensajeParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 100)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(mensajeParam);

                        SqlParameter salarioIdParam = new SqlParameter("@SalarioId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(salarioIdParam);

                        command.ExecuteNonQuery();
                        message = (string)mensajeParam.Value;
                        salarioId = (int)salarioIdParam.Value;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al Adjuntar " + e.Message);
                throw;
            }
            return salarioId;
        }/**/
        public static void GuardarConceptoSalario(int salarioID, int conceptoID)
        {
            using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GuardarConceptoSalario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SalarioID", salarioID);
                    cmd.Parameters.AddWithValue("@ConceptoID", conceptoID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public static List<ConceptoNominaModel> GetConceptosNomina()
        {
            var conceptos = new List<ConceptoNominaModel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ConceptosNominas",conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var concepto = new ConceptoNominaModel
                                {
                                    conceptoId = reader.GetInt32(reader.GetOrdinal("ConceptoID")),
                                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                    TipoNomina = reader.GetString(reader.GetOrdinal("Tipo"))
                                };
                                conceptos.Add(concepto);
                            }
                        }

                    }

                }
                return conceptos;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<SalarioEmpleadoModel> ObtenerSalarios()
        {
            var salariosList = new List<SalarioEmpleadoModel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ObtenerSalarios",conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var salarios = new SalarioEmpleadoModel
                                {
                                    SalarioID = reader.GetInt32(reader.GetOrdinal("Codigo_Salario")),
                                    EmpleadoID = reader.GetInt32(reader.GetOrdinal("EmpleadoID")),
                                    Nombre_Empleado = reader.GetString(reader.GetOrdinal("Nombre")),
                                    cedula = reader.GetString(reader.GetOrdinal("Cedula")),
                                    SalarioBase = reader.GetDecimal(reader.GetOrdinal("SalarioBase"))
                                };
                                salariosList.Add(salarios);
                            }

                        }
                    }

                }
                return salariosList;

            }
            catch (Exception e)
            {
                Console.WriteLine("Error Obtener Salarios " + e.Message);
                throw;
            }
        }

        public static List<SalarioEmpleadoModel> ObtenerSalariosReporte()
        {
            List<SalarioEmpleadoModel> salarios = new List<SalarioEmpleadoModel>();
         

            using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("sp_ObtenerSalarios", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SalarioEmpleadoModel salario = new SalarioEmpleadoModel
                            {
                                SalarioID = reader.GetInt32(reader.GetOrdinal("Codigo_Salario")),
                                EmpleadoID = reader.GetInt32(reader.GetOrdinal("EmpleadoID")),
                                Nombre_Empleado = reader.GetString(reader.GetOrdinal("Nombre")),
                                cedula = reader.GetString(reader.GetOrdinal("Cedula")),
                                SalarioBase = reader.GetDecimal(reader.GetOrdinal("SalarioBase"))
                            };
                            salarios.Add(salario);
                        }
                    }
                }
            }

            // Depurar los datos
            Console.WriteLine("Datos obtenidos:");
            foreach (var salario in salarios)
            {
                Console.WriteLine($"Nombre: {salario.Nombre_Empleado}, SalarioBase: {salario.SalarioBase}");
            }

            return salarios;
        }


    }
}
