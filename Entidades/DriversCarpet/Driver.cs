using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Entidades.DriversCarpet
{
    [Serializable]
    [DataContract]
    public class Driver
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string lm1 { get; set; }
        [DataMember]
        public string lm2 { get; set; }
        [DataMember]
        public DateTime birth { get; set; }
        [DataMember]
        public DateTime hireDate { get; set;}
        [DataMember]
        public DateTime lastModD { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
        public string phone { get; set; }
        [DataMember]
        public int settlement { get; set; }
        [DataMember]
        public int st1 { get; set; }
        [DataMember]
        public int st2 { get; set; }
        [DataMember]
        public int st3 { get; set; }
        [DataMember]
        public int contactDrivers { get; set; }
        [DataMember]
        public int extNumber { get; set; }
        [DataMember]
        public int admin { get; set; }
        [DataMember]
        public DateTime licenseEx { get; set; }
        [DataMember]
        public int ingressPay { get; set; }
        [DataMember]
        public int status { get; set; }
        [DataMember]
        public string statusS { get; set; }
        [DataMember]
        public string street1 { get; set; }
        [DataMember]
        public string street2 { get; set; }
        [DataMember]
        public string street3 { get; set; }
        [DataMember]
        public string settlementS { get; set;}
        [DataMember]
        public string adminName { get; set; }

    }
    public class listDrivers: List<Driver> { }
}
