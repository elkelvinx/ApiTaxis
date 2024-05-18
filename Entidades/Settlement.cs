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
    public class settlement
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
    }
    public class listSettle: List<settlement> { }
}
