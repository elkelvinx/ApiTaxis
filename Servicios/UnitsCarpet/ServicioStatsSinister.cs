using System;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;

namespace Servicios.UnitsCarpet
{
    public class ServicioStatsSinister
    {
        // Inicializa la tabla con todos los siniestros existentes
        public void InicializarSinistersMonthlyStats()
        {
            string query = @"
                INSERT INTO SinistersMonthlyStats (eventYear, eventMonth, totalSinisters)
                SELECT 
                    YEAR(dateEvent) AS eventYear,
                    MONTH(dateEvent) AS eventMonth,
                    COUNT(*) AS totalSinisters
                FROM sinisters
                WHERE dateEvent IS NOT NULL
                GROUP BY YEAR(dateEvent), MONTH(dateEvent);";

            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.ExecuteNonQuery();
            }
        }

        // Sumar un siniestro en el mes del evento
        public void ActualizarSinisterStatusMensual(DateTime dateEvent)
        {
            int year = dateEvent.Year;
            int month = dateEvent.Month;
            int yyyymm = year * 100 + month;

            string query = @"
                UPDATE SinistersMonthlyStats
                SET totalSinisters = totalSinisters + 1
                WHERE id = @yyyymm;

                IF @@ROWCOUNT = 0
                BEGIN
                    INSERT INTO SinistersMonthlyStats (eventYear, eventMonth, totalSinisters)
                    VALUES (@year, @month, 1);
                END";

            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@yyyymm", yyyymm);
                cmd.ExecuteNonQuery();
            }
        }

        // Restar un siniestro en el mes del evento
        public void DisminuirSinisterMonthlyStats(DateTime dateEvent)
        {
            int year = dateEvent.Year;
            int month = dateEvent.Month;
            int yyyymm = year * 100 + month;

            string query = @"
                UPDATE SinistersMonthlyStats
                SET totalSinisters = totalSinisters - 1
                WHERE id = @yyyymm AND totalSinisters > 0;";

            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@yyyymm", yyyymm);
                cmd.ExecuteNonQuery();
            }
        }


    }
}
