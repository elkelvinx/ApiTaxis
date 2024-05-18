using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Entidades.Arrays
{
    [Serializable]
    [DataContract]
    public class Arrays
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        
    }
    public class listArray: List<Arrays> { }
}
