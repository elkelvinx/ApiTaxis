using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Entidades
{

    [Serializable]
    [DataContract]
    public class Admin
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
        public int st1 { get; set; }
        [DataMember]
        public int st2 { get; set; }
        [DataMember]
        public int st3 { get; set; }
        [DataMember] 
        public int settlement { get; set; }
        [DataMember]
        public int extNumber { get; set; }
        [DataMember]
        public DateTime birthDate { get; set; }
        [DataMember]
        public DateTime registerD { get; set; }
        [DataMember]
        public DateTime lastModDate { get; set; }
        [DataMember]
        public string mail { get; set; }
        [DataMember]
        public string street1 { get; set; }
        [DataMember]
        public string street2 { get; set;}
        [DataMember]
        public string street3 { get; set;}
        [DataMember]
        public string settlementS { get; set;}
    }
    public class listAdmin : List<Admin> { }
}
