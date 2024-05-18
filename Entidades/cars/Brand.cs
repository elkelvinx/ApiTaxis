using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Entidades.cars
{
    [Serializable]
    [DataContract]
    public class Brand
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
    }
    public class listBrand : List<Brand> { }
}
