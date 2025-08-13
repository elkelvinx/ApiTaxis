using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Entidades.Cars
{
    [Serializable]
    [DataContract]
    public class ModelsCar
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string brandName { get; set; }
        [DataMember]
        public int idBrand { get; set; }

    }
    public class listModelsCar : List<ModelsCar> { }
}
