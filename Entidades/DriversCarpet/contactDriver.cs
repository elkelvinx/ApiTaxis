using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.DriversCarpet
{
    [Serializable]
    [DataContract]
    public class contactDriver
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
        public int extNumber { get; set; }
        [DataMember]
        public int relation { get; set; }
        [DataMember]
        public string relationS { get; set; }
        //Cosas para reportes
        [DataMember]
        public string street1 { get; set; }
        [DataMember]
        public string street2 { get; set; }
        [DataMember]
        public string street3 { get; set; }
        [DataMember]
        public string settlementS { get; set; }

    }
    public class listContactDriver : List<contactDriver> { }
}
