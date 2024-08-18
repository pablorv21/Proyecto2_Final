using Sistema_Nominas.Models;
using Syncfusion.Compression;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Sistema_Nominas.Util
{
    public class ConceptoNominaDAL
    {
        /**
         * Método AgregarNuevoConcepto
         * 
         * Parámetros:: 
         * - concepto: Objeto de tipo ConceptoNominaModel que contiene la información del concepto a agregar.
         * 
         * Funcionalidad:: 
         * - Inserta un nuevo concepto de nómina en la base de datos utilizando un procedimiento almacenado.
         * - Retorna `true` si la inserción fue exitosa; de lo contrario, retorna `false`.
         * 
         * Excepciones:: 
         * - Lanza una excepción si ocurre un error durante la conexión a la base de datos o la ejecución del procedimiento almacenado.
         */
        public static Boolean AgregarNuevoConcepto(ConceptoNominaModel concepto)
        {
			Boolean Resultado=false;
			try
			{
				using(SqlConnection conn = new SqlConnection(ConexionString.Conexion))
				{
					conn.Open();
					using(SqlCommand cmd=new SqlCommand("SP_InsertarConceptoNomina",conn))
					{
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nombre", concepto.Nombre);
                        cmd.Parameters.AddWithValue("@Tipo", concepto.TipoNomina);
                        /*Manejo de la respuesta */
                        SqlParameter outputParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);
                        cmd.ExecuteNonQuery();

                        string mensajeSalida = outputParam.Value.ToString();
                        Resultado = mensajeSalida.StartsWith("Nuevo Concepto creado");
                        return Resultado;
                    }

				}
			}
			catch (Exception e)
			{
                Console.Write("Error al ingresar el concepto " + e.Message);

				throw;
			}

        }
        /*Calcular*/
        private static Decimal CalcularCCSS(Decimal SalarioBruto,Double Porcentaje_Css)
        {
            return (SalarioBruto * (Decimal)Porcentaje_Css) / 100;
        }



        public static void GenerarNominas()
        {
            Decimal CCSS = 0;
            Decimal SalarioNeto = 0;
            try
            {
                DateTime fechaActual = DateTime.Now;
                DateTime primerDiaDelMes = new DateTime(fechaActual.Year, fechaActual.Month, 1);
                int ultimoDia = DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month);
                DateTime ultimoDiaDelMes = new DateTime(fechaActual.Year, fechaActual.Month, ultimoDia);
                List<SalarioEmpleadoModel> SalariosList = SalariosEmpleadosDAL.ObtenerSalarios();
                foreach (var item in SalariosList)
                {
                    Decimal impuesto = calcular_Hacienda(item.SalarioBase);
                    CCSS = CalcularCCSS(item.SalarioBase, 10.67);
                    Decimal caja = CCSS;
                    Console.WriteLine("REbajo de la caja " + CCSS);
                    Console.WriteLine("Hacienda " + impuesto);
                    var NominaEmpleadoModel = new NominaEmpleadoModel
                    {
                        EmpleadoID = item.EmpleadoID,
                        PeriodoInicio = primerDiaDelMes,
                        PeriodoFin = ultimoDiaDelMes,
                        FechaGeneracion = fechaActual,
                        SalarioBase = item.SalarioBase,
                        TotalIngresos = item.SalarioBase,
                        TotalDeducciones = CCSS + impuesto,
                        TotalNeto = item.SalarioBase - (CCSS + impuesto)
                    };
                    ConceptoNominaDAL.AgregarNominaEmpleado(NominaEmpleadoModel);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        private static Decimal calcular_Hacienda(Decimal SalarioBruto)
        {
            decimal impuesto = 0;
            try
            {
                if (SalarioBruto <= 929000)
                {
                    // Exento
                    impuesto = 0;
                }
                else if (SalarioBruto <= 1363000)
                {
                    // 10% sobre el exceso de ₡929.000,00
                    impuesto = (SalarioBruto - 929000) * 0.10m;
                }
                else if (SalarioBruto <= 2392000)
                {
                    // 15% sobre el exceso de ₡1.363.000,00
                    impuesto = (1363000 - 929000) * 0.10m + (SalarioBruto - 1363000) * 0.15m;
                }
                else if (SalarioBruto <= 4783000)
                {
                    // 20% sobre el exceso de ₡2.392.000,00
                    impuesto = (1363000 - 929000) * 0.10m + (2392000 - 1363000) * 0.15m + (SalarioBruto - 2392000) * 0.20m;
                }
                else
                {
                    // 25% sobre el exceso de ₡4.783.000,00
                    impuesto = (1363000 - 929000) * 0.10m + (2392000 - 1363000) * 0.15m + (4783000 - 2392000) * 0.20m + (SalarioBruto - 4783000) * 0.25m;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("error en Haciendoa " + e.ToString());
                throw;
            }
            return impuesto;
        }
      

        public static Boolean AgregarNominaEmpleado(NominaEmpleadoModel model) 
        {
            bool resultado = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_InsertarNomina", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        Console.WriteLine("model.PeriodoInicio " + model.PeriodoInicio);
                        Console.WriteLine(" model.PeriodoFin " + model.PeriodoFin);
                        Console.WriteLine("@FechaGeneracion" + DateTime.Now);

                        cmd.Parameters.AddWithValue("@EmpleadoID", model.EmpleadoID);
                        cmd.Parameters.AddWithValue("@PeriodoInicio", model.PeriodoInicio);
                        cmd.Parameters.AddWithValue("@PeriodoFin", model.PeriodoFin);                        
                        cmd.Parameters.AddWithValue("@FechaGeneracion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@SalarioBase", model.SalarioBase);

                        cmd.Parameters.AddWithValue("@TotalIngresos", model.TotalIngresos);
                        cmd.Parameters.AddWithValue("@TotalDeducciones", model.TotalDeducciones);
                        cmd.Parameters.AddWithValue("@TotalNeto", model.TotalNeto);

                        SqlParameter outputNominaID = new SqlParameter("@NominaID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputNominaID);

                        SqlParameter mensajeParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1000);
                        mensajeParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(mensajeParam);

                        cmd.ExecuteNonQuery();

                        int nominaID = (int)cmd.Parameters["@NominaID"].Value;
                        AdjuntarLosDetalles(nominaID,6, model.SalarioBase, model.SalarioBase); 

                        string mensaje = mensajeParam.Value.ToString();
                        resultado = mensaje.Contains("correctamente");
                        return resultado;
                        Console.WriteLine(mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                throw;
            }
            return resultado;

        }

        private static Boolean AdjuntarLosDetalles(int NominaId, int conceptoId, Decimal monto, Decimal CCSS)
        {
            Boolean resultado = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_InsertarDetalleNomina",conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NominaID", NominaId);
                        cmd.Parameters.AddWithValue("@ConceptoID", conceptoId);
                        cmd.Parameters.AddWithValue("@Monto", monto);

                        SqlParameter mensajeParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1000);
                        mensajeParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(mensajeParam);
                        cmd.ExecuteNonQuery();

                        string mensaje = mensajeParam.Value.ToString();
                        resultado = mensaje.Contains("exitosamente");
                        CrearRebajoCCSS(NominaId, CCSS);
                        return resultado;

                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return resultado;

        }

        private static void CrearRebajoCCSS(int NominaId, Decimal monto)
        {
            Boolean resultado = false;
            Decimal CajaCCSS = (monto * 10.67m) / 100;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionString.Conexion))
                {
                    conn.Open();
                    using (SqlCommand cmdSeguro = new SqlCommand("sp_InsertarDetalleNomina", conn))
                    {
                        cmdSeguro.CommandType = CommandType.StoredProcedure;
                        cmdSeguro.Parameters.AddWithValue("@NominaID", NominaId);
                        cmdSeguro.Parameters.AddWithValue("@ConceptoID", 2); 
                        cmdSeguro.Parameters.AddWithValue("@Monto", CajaCCSS); 

                        SqlParameter mensajeParamSeguro = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1000);
                        mensajeParamSeguro.Direction = ParameterDirection.Output;
                        cmdSeguro.Parameters.Add(mensajeParamSeguro);
                        cmdSeguro.ExecuteNonQuery();

                        string mensajeSeguro = mensajeParamSeguro.Value.ToString();
                        resultado = resultado && mensajeSeguro.Contains("exitosamente"); // Se actualiza el resultado para ambos casos
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en CCSS " + e.Message);
                throw;
            }
        }
                   





    }
}
