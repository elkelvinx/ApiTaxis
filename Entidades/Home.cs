using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Entidades
{
    // Rango de fechas(para endpoints de gráficas)
    [DataContract]
    public class DateRangeDto
    {
        [DataMember] public DateTime? StartDate { get; set; }
        [DataMember] public DateTime? EndDate { get; set; }
    }

    // Punto mensual genérico (para series)
    [Serializable]
    [DataContract]
    public class MonthPoint
    {
        [DataMember] public int Year { get; set; }
        [DataMember] public int Month { get; set; }
        [DataMember] public int Total { get; set; }
    }
    public class MonthPointList : List<MonthPoint> { }


    [Serializable]
    [DataContract]
    public class CountMonthly
    {
        [DataMember] public int Current { get; set; }
        [DataMember] public int Previous { get; set; }
    }
    public class KpiData
    {
        [DataMember] public int CurrentMonth { get; set; }
        [DataMember] public int PreviousMonth { get; set; }
        [DataMember] public double Percentage { get; set; } 
    }
}