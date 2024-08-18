using Sistema_Nominas.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Nominas.Util
{
    public class EmpleadosDAL
    {
        /**
         * Metodo ObtenerTodosLosEmpleados
         * Parametros:: Cadena de Conexion
         * Funcionalidad:: Retorna todos los empleados 
         * */
        public static List<EmpleadoModel> ObtenerTodosLosEmpleados(String Conexion)
        {
            var EmpleadoList = new List<EmpleadoModel>();
            using (SqlConnection conn = new SqlConnection(Conexion))
            {
                using (SqlCommand command = new SqlCommand("SP_BUSCAREMPLEADOS", conn))
                {
                    conn.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime FechaContratacion2 = reader.GetDateTime(reader.GetOrdinal("FechaContratacion"));

                            DateTime? FechaTerminacion2 = reader.IsDBNull(reader.GetOrdinal("FechaTerminacion"))
                                                          ? (DateTime?)null:reader.GetDateTime(reader.GetOrdinal("FechaTerminacion"));

                            var empModel = new EmpleadoModel
                            {
                                EmpleadoID = reader.GetInt32(reader.GetOrdinal("EmpleadoID")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                                CorreoElectronico = reader.GetString(reader.GetOrdinal("CorreoElectronico")),
                                Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                                Direccion = reader.GetString(reader.GetOrdinal("Direccion")),
                                FechaContratacion = DateOnly.FromDateTime(FechaContratacion2),
                                FechaTerminacion = FechaTerminacion2.HasValue? DateOnly.FromDateTime(FechaTerminacion2.Value):null,
                                RolID = reader.GetInt32(reader.GetOrdinal("RolID")),
                                Cedula = reader.GetString(reader.GetOrdinal("Cedula"))
                            };
                            EmpleadoList.Add(empModel);
                        }
                    }
                }
            }
            return EmpleadoList;
        }
        /**
         * Metodo ObtenerEmpleadoPorID
         * Parametros:: REcibe el ID del empleado
         * Funcionalidad:: Retorna el empleado 
         * */
        public static EmpleadoModel ObtenerEmpleadoPorID(int empId)
        {
            var EmpleadoList = new EmpleadoModel();
            using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("SP_BuscarPorEmpleadoID", conn))
                {
                    Console.WriteLine("Entro Por ID");

                    command.Parameters.AddWithValue("@EmpleadoID", empId);
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EmpleadoList = new EmpleadoModel
                            {
                                EmpleadoID = reader.GetInt32(reader.GetOrdinal("EmpleadoID")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                                CorreoElectronico = reader.GetString(reader.GetOrdinal("CorreoElectronico")),
                                Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                                Direccion = reader.GetString(reader.GetOrdinal("Direccion")),
                                RolID = reader.GetInt32(reader.GetOrdinal("RolID")),
                                Cedula = reader.GetString(reader.GetOrdinal("Cedula")),
                                NombreRole = reader.GetString(reader.GetOrdinal("NombreRol")),
                                DescriptionRole = reader.GetString(reader.GetOrdinal("DescripcionRole"))
                            };                           
                        }
                    }
                }
            }
            return EmpleadoList;
        }
        /**
          * Metodo CrearNuevoEmpleado
          * Parametros:: Objecto de Tipo EmpleadoModel
          * Funcionalidad:: Crea Empleados 
          * */
        public static int CrearNuevoEmpleado(EmpleadoModel EmpModel)
        {
            int EmpleadoID = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("sp_InsertarEmpleado", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Nombre", EmpModel.Nombre);
                        command.Parameters.AddWithValue("@Apellido", EmpModel.Apellido);
                        command.Parameters.AddWithValue("@CorreoElectronico", EmpModel.CorreoElectronico);

                        command.Parameters.AddWithValue("@Telefono", EmpModel.Telefono);
                        command.Parameters.AddWithValue("@Direccion", EmpModel.Direccion);
                        command.Parameters.AddWithValue("@RolID", EmpModel.RolID);
                        command.Parameters.AddWithValue("@Cedula", EmpModel.Cedula);
                        
                        SqlParameter outputParameter = new SqlParameter();
                        outputParameter.ParameterName = "@EmpleadoID";
                        outputParameter.SqlDbType = SqlDbType.Int;
                        outputParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputParameter);

                        Console.WriteLine("@Nombre = " + command.Parameters["@Nombre"].Value);
                        command.ExecuteNonQuery();
                        EmpleadoID = (int)outputParameter.Value;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Nuevo Empleado " + ex.Message);
                Console.WriteLine("Nuevo Empleado " + ex.StackTrace);
                throw;
            }
            return EmpleadoID;  
        }

        /**
          * Metodo       ::: EditarEmpleado
          * Parametros   :: Objecto de Tipo EmpleadoModel
          * Funcionalidad:: Toma el ID del Empleado y lo actualiza.
        * */
        public static bool EditarEmpleado(EmpleadoModel EmpModel)
        {
            bool resultado = false;
            Console.WriteLine(EmpModel.EmpleadoID);
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ActualizarEmpleado", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmpleadoID", EmpModel.EmpleadoID);
                        cmd.Parameters.AddWithValue("@Nombre", EmpModel.Nombre);
                        cmd.Parameters.AddWithValue("@Apellido", EmpModel.Apellido);
                        cmd.Parameters.AddWithValue("@CorreoElectronico", EmpModel.CorreoElectronico);
                        cmd.Parameters.AddWithValue("@Telefono", EmpModel.Telefono);
                        cmd.Parameters.AddWithValue("@Direccion", EmpModel.Direccion);
                        cmd.Parameters.AddWithValue("@RolID", EmpModel.RolID);
                        cmd.Parameters.AddWithValue("@Cedula", EmpModel.Cedula);

                        int numeroAfectados = cmd.ExecuteNonQuery();
                        Console.WriteLine("numeroAfectados " + numeroAfectados);

                        if (numeroAfectados > 0 || numeroAfectados == -1)
                        {
                            resultado = true;
                        }
                        return resultado;
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("SQL Error: " + e.Message);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("General Error: " + e.Message);
                return false;
            }
        }


        /**
        * Metodo CargarRoles
        * Parametros:: N/A
        * Funcionalidad:: DeVuelve los roles para realizar la creacion de los Empleados
        * */
        public static List<RolesModel> ObtenerLosRoles()
        {
            List<RolesModel> rolesModels = new List<RolesModel>();
            Console.WriteLine("rolesModels");
            using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("SP_BUSCARROLES", conn))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RolesModel rolesModel = new RolesModel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("RolID")),
                                NombreRol = reader.GetString(reader.GetOrdinal("NombreRol")),
                            };
                            rolesModels.Add(rolesModel);
                        }
                    }
                }

            }
            Console.WriteLine(rolesModels);
            return rolesModels;
        }
    }
}
