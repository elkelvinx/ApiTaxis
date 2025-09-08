using System;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;

namespace Servicios.DriverServices
{
    public class ServicioStatsSinister
    {
        public void InicializarDriversMonthlyStats()
        {
            string query = @"
              INSERT INTO DriversMonthlyStats (ingressYear, ingressMonth, totalDrivers)
              SELECT 
                  YEAR(hireDate) AS ingressYear,
                  MONTH(hireDate) AS ingressMonth,
                  COUNT(*) AS totalDrivers
              FROM drivers
              WHERE hireDate IS NOT NULL
              GROUP BY YEAR(hireDate), MONTH(hireDate);";

            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void ActualizarDriversStatusMensual(DateTime hireDate)
        {
            int year = hireDate.Year;
            int month = hireDate.Month;
            int yyyymm = year * 100 + month;

            string updateQuery = @"
              UPDATE DriversMonthlyStats
              SET totalDrivers = totalDrivers + 1
              WHERE id = @yyyymm;

             IF @@ROWCOUNT = 0
              BEGIN
             INSERT INTO DriversMonthlyStats (ingressYear, ingressMonth, totalDrivers)
             VALUES (@year, @month, 1);
              END";

            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(updateQuery, con))
            {
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@yyyymm", yyyymm);
                cmd.ExecuteNonQuery();
            }
        }


        public void DisminuirDriversMonthlyStats(DateTime hireDate)
        {
            int year = hireDate.Year;
            int month = hireDate.Month;

            string updateQuery = @"
              UPDATE DriversMonthlyStats
              SET totalDrivers = totalDrivers - 1
              WHERE (ingressYear * 100 + ingressMonth) = @yyyymm
                 AND totalDrivers > 0;";

            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(updateQuery, con))
            {
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.ExecuteNonQuery();
            }
        }


    }
}
