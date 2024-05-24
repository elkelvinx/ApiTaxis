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
    public class Street
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }

    }
    public class listStreet: List<Street> { }
}
