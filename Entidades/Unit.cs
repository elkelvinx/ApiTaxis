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
    public class Unit
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int ecoNumber { get; set; }
      //[DataMember]
      //  public int brand { get; set; }
        [DataMember]
        public int model { get; set; }//
        [DataMember] 
        public int yearModel{ get; set; }
        [DataMember]
        public string color { get; set; }
        [DataMember]   
        public string serie { get; set; }
        [DataMember]
        public string motor { get; set; }
        [DataMember]
        public string plate{ get; set; }
        [DataMember]
        public DateTime registerDate { get; set; }
        [DataMember]
        public DateTime lastModDate { get; set; }
        [DataMember]
        public DateTime expInsurance {  get; set; }
        [DataMember]
        public int admin{ get; set; }
        [DataMember]
        public string adminName { get; set; }
        [DataMember]
         public string brandName { get; set; }
        [DataMember]
        public string modelName { get; set; }
    }
    public class listUnit : List<Unit> { }
}
