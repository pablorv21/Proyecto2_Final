using Sistema_Nominas.Models;
using System.Data.SqlClient;

namespace Sistema_Nominas.Util
{
	public class NonimaDAL
	{
		public static List<NominaViewModel> ObtenerDatosNomina( )
		{
			 List<NominaViewModel> lstModel = new List<NominaViewModel>();


			using (var connection = new SqlConnection(ConexionString.Conexion))
			{
				connection.Open();
				using (var command = new SqlCommand(@"SELECT E.Nombre AS NombreEmpleado,N.PeriodoInicio, N.NominaID,
                                                    N.PeriodoFin,N.SalarioBase,N.TotalIngresos, 
                                                    N.TotalDeducciones,N.TotalNeto
                                                    FROM Nominas N
                                                    JOIN Empleados E ON N.EmpleadoID = E.EmpleadoID", connection))
				{
					using (var reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							var Nomina = new NominaViewModel {
								NominaID = reader.GetInt32(reader.GetOrdinal("NominaID")),
								NombreEmpleado = reader.GetString(reader.GetOrdinal("NombreEmpleado")),
								PeriodoInicio = reader.GetDateTime(reader.GetOrdinal("PeriodoInicio")),
								PeriodoFin = reader.GetDateTime(reader.GetOrdinal("PeriodoFin")),
								SalarioBase = reader.GetDecimal(reader.GetOrdinal("SalarioBase")),
								TotalIngresos = reader.GetDecimal(reader.GetOrdinal("TotalIngresos")),
								TotalDeducciones = reader.GetDecimal(reader.GetOrdinal("TotalDeducciones")),
								TotalNeto = reader.GetDecimal(reader.GetOrdinal("TotalNeto"))
							};
							lstModel.Add(Nomina);
						}
					}
				}

				
			}
			return lstModel;
		}
	}
}
