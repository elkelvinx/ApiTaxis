using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices.ComTypes;

namespace Servicios
{
    public class ServicioHome
    {
        // ---------- KPI: pares (current/previous) desde tablas de stats ----------

        public CountMonthly GetDriversMonthlyPair()
        {
            const string sql = @"
                SELECT ingressYear, ingressMonth, totalDrivers
                FROM DriversMonthlyStats
                WHERE (ingressYear = YEAR(GETDATE()) AND ingressMonth = MONTH(GETDATE()))
                   OR (ingressYear = YEAR(DATEADD(MONTH, -1, GETDATE())) 
                       AND ingressMonth = MONTH(DATEADD(MONTH, -1, GETDATE())))";

            var data = new CountMonthly();

            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            using (SqlDataReader r = cmd.ExecuteReader())
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;

                while (r.Read())
                {
                    int year = (int)r["ingressYear"];
                    int month = (int)r["ingressMonth"];
                    int totalDrivers = (int)r["totalDrivers"];

                    if (year == currentYear && month == currentMonth) data.Current = totalDrivers;
                    else data.Previous = totalDrivers;
                }
            }
            return data;
        }

        public CountMonthly GetSinistersMonthlyPair()
        {
            const string sql = @"
                SELECT eventYear, eventMonth, totalSinisters
                FROM SinistersMonthlyStats
                WHERE (eventYear = YEAR(GETDATE()) AND eventMonth = MONTH(GETDATE()))
                   OR (eventYear = YEAR(DATEADD(MONTH, -1, GETDATE())) 
                       AND eventMonth = MONTH(DATEADD(MONTH, -1, GETDATE())))";

            var data = new CountMonthly();

            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            using (SqlDataReader r = cmd.ExecuteReader())
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;

                while (r.Read())
                {
                    int year = (int)r["eventYear"];
                    int month = (int)r["eventMonth"];
                    int totalDrivers = (int)r["totalSinisters"];

                    if (year == currentYear && month == currentMonth) data.Current = totalDrivers;
                    else data.Previous = totalDrivers;
                }
            }
            return data;
        }

        // ---------- KPI finales (ya con % calculado en API) ----------

        public KpiData CalculateCurrentDriversKpi()
        {
            var data = GetDriversMonthlyPair();
            return BuildKpi(data.Current, data.Previous);
        }

        public KpiData CalculateCurrentSinistersKpi()
        {
            var data = GetSinistersMonthlyPair();
            return BuildKpi(data.Current, data.Previous);
        }

        // ---------- Utilidades privadas ----------
        private static int ToKey(DateTime d) => d.Year * 100 + d.Month;
        private static KpiData BuildKpi(int current, int previous)
        {
            double percentage;
            if (previous == 0)
                percentage = current > 0 ? 100.0 : 0.0;
            else
                percentage = ((double)(current - previous) / previous) * 100.0;

            return new KpiData
            {
                CurrentMonth = current,
                PreviousMonth = previous,
                Percentage = Math.Round(percentage, 2)
            };
        }
        private static void NormalizeRange(ref DateTime? start, ref DateTime? end)
        {
            if (!end.HasValue) end = DateTime.Today;
            if (!start.HasValue) start = end.Value.AddMonths(-6);

            start = new DateTime(start.Value.Year, start.Value.Month, 1, 0, 0, 0);
            end = new DateTime(end.Value.Year, end.Value.Month, 1, 23, 59, 59);
        }

        public MonthPointList GetDriversSeries(DateTime? startDateRange, DateTime? endDateRange)
        {
            NormalizeRange(ref startDateRange, ref endDateRange);
            var startKey = ToKey(startDateRange.Value);
            var endKey = ToKey(endDateRange.Value);

            string consulta = @"
                SELECT ingressYear AS [Year], ingressMonth AS [Month], totalDrivers AS [Total]
                FROM DriversMonthlyStats
                WHERE (ingressYear * 100 + ingressMonth) BETWEEN @startKey AND @endKey
                ORDER BY ingressYear, ingressMonth;";

            var map = new Dictionary<int, int>();
            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(consulta, con))
            {
                cmd.Parameters.Add("@startKey", SqlDbType.Int).Value = startKey;
                cmd.Parameters.Add("@endKey", SqlDbType.Int).Value = endKey;

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        int y = (int)r["Year"];
                        int m = (int)r["Month"];
                        int t = (int)r["Total"];
                        map[y * 100 + m] = t;
                    }
                }
            }
            // Rellenar meses faltantes con 0 para una serie continua
            var list = new MonthPointList();
            var cursor = new DateTime(startDateRange.Value.Year, startDateRange.Value.Month, 1);
            var end = new DateTime(endDateRange.Value.Year, endDateRange.Value.Month, 1);

            int lastValue = 0;
            while (cursor <= end)
            {
                int key = cursor.Year * 100 + cursor.Month;

                if (map.TryGetValue(key, out var v))
                    lastValue = v;  // si hay valor para este mes, lo usamos y actualizamos lastValue

                list.Add(new MonthPoint
                {
                    Year = cursor.Year,
                    Month = cursor.Month,
                    Total = lastValue
                });

                cursor = cursor.AddMonths(1);
            }

            return list;
        }
        public MonthPointList GetSinistersSeries(DateTime? startDate, DateTime? endDate)
        {
            NormalizeRange(ref startDate, ref endDate);
            var startKey = ToKey(startDate.Value);
            var endKey = ToKey(endDate.Value);

            const string sql = @"
                SELECT eventYear AS [Year], eventMonth AS [Month], totalSinisters AS [Total]
                FROM SinistersMonthlyStats
                WHERE (eventYear * 100 + eventMonth) BETWEEN @startKey AND @endKey
                ORDER BY eventYear, eventMonth;";

            var map = new Dictionary<int, int>();
            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@startKey", SqlDbType.Int).Value = startKey;
                cmd.Parameters.Add("@endKey", SqlDbType.Int).Value = endKey;

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        int y = (int)r["Year"];
                        int m = (int)r["Month"];
                        int t = (int)r["Total"];
                        map[y * 100 + m] = t;
                    }
                }
            }

            var list = new MonthPointList();
            var cursor = new DateTime(startDate.Value.Year, startDate.Value.Month, 1);
            var end = new DateTime(endDate.Value.Year, endDate.Value.Month, 1);

            while (cursor <= end)
            {
                int key = cursor.Year * 100 + cursor.Month;
                list.Add(new MonthPoint
                {
                    Year = cursor.Year,
                    Month = cursor.Month,
                    Total = map.TryGetValue(key, out var v) ? v : 0
                });
                cursor = cursor.AddMonths(1);
            }
            return list;
        }
    }
}
