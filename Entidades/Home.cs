using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    [Serializable]
    [DataContract]
    public class HomeDriversRise
    {
        [DataMember]
        public int year { get; set; }
        [DataMember]
        public int month { get; set; }
        [DataMember]
        public int totalDrivers { get; set; }
    }
    public class listHomeDriversRise : List<HomeDriversRise> { }
    public class DateRangeDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
