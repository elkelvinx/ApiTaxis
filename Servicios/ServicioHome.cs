using Entidades;
using System;
using System.Data.SqlClient;

namespace Servicios
{
    public class ServicioHome
    {
        public int GetDriversCount()
        {
            int count = 0;
            string consulta = "SELECT COUNT(*) FROM drivers";

            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(consulta, con))
            {
                count = (int)cmd.ExecuteScalar();
            }

            return count;
        }

        public int GetSinistersThisMonth()
        {
            int count = 0;
            string consulta = "Select COUNT(*) from Sinisters where MONTH(dateEvent) = MONTH(GETDATE())";
            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(consulta, con))
            {
                count = (int)cmd.ExecuteScalar();
            }
            return count;
        }

        public listHomeDriversRise homeDriversRises(DateTime? startDateRange, DateTime? endDateRange)
        {
            var list = new listHomeDriversRise();
            // Si no viene fecha de inicio y fin desde Angular, calculamos últimos 7 meses
            if (!endDateRange.HasValue)
                endDateRange = DateTime.Today; // Fecha actual

            if (!startDateRange.HasValue)
                startDateRange = endDateRange.Value.AddMonths(-6); // 6 meses hacia atrás

            // Normalizar horas
            startDateRange = new DateTime(startDateRange.Value.Year, startDateRange.Value.Month, startDateRange.Value.Day, 0, 0, 0);
            endDateRange = new DateTime(endDateRange.Value.Year, endDateRange.Value.Month, endDateRange.Value.Day, 23, 59, 59, 997);

            string consulta = @"
                     SELECT 
                             YEAR(hireDate) AS year,
                             MONTH(hireDate) AS month,
                             COUNT(*) AS totalDrivers
                           FROM drivers
                           WHERE hireDate >= @startDateRange
                            AND hireDate <= @endDateRange
                           GROUP BY YEAR(hireDate), MONTH(hireDate)
                           ORDER BY year, month;";
            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(consulta, con))
            {
                cmd.Parameters.AddWithValue("@startDateRange", startDateRange);
                cmd.Parameters.AddWithValue("@endDateRange", endDateRange);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new HomeDriversRise
                    {
                        year = Convert.ToInt32(reader["year"]),
                        month = Convert.ToInt32(reader["month"]),
                        totalDrivers = Convert.ToInt32(reader["totalDrivers"])
                    });
                }
            }
            return list;
        }
    }
}
