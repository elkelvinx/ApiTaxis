using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.cars
{
    [Serializable]
    [DataContract]
    public class Sinister
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int typeSinister { get; set; }
        [DataMember]
        public int idUnit { get; set; }
        [DataMember]
        public int ecoNumber { get; set; }
        [DataMember]
        public DateTime dateEvent {  get; set; }
        [DataMember]
        public string typeInsurance { get; set; }
        [DataMember]
        public string workShop { get; set; }
        [DataMember]
        public int admin { get; set; }
        [DataMember] 
        public int driver { get; set; }
        [DataMember]
        public string crashCounterPart { get; set; }
        [DataMember]
        public int personInvolved { get; set; }
        [DataMember]
        public string personInvolvedS { get; set; }
        [DataMember]
        public int insuranceAplication { get; set; }
        [DataMember]
        public string insuranceAplicationS {  get; set; }
        [DataMember]
        public int insurance { get; set; }
        [DataMember]
        public bool winOrLoose { get; set; }
        [DataMember]
        public string winOrLooseS { get; set; }
        [DataMember]
        public string observations { get; set; }
        [DataMember]
        public int settlement { get; set; }
        [DataMember]
        public int st1 { get; set; }
        [DataMember]
        public int st2 { get; set; }
        [DataMember]
        public int st3 { get; set; }
        [DataMember]
        public string street1 { get; set; }
        [DataMember]
        public string street2 { get; set; }
        [DataMember]
        public string street3 { get; set; }
        [DataMember]
        public string settlementS { get; set; }
        [DataMember]
        public string typeSinisterS { get; set; }
        [DataMember]
        public string adminS { get; set; }
        [DataMember]
        public string driverS { get; set; }
        [DataMember]
        public string insuranceS { get; set; }
    }

    public class listSinister : List<Sinister> { }
}
