using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Entidades.Logs
{
    [Serializable]
    [DataContract]
    public class historyLogIn
    {
            [DataMember]
            public int id { get; set; }
            [DataMember]
            public int idUser { get; set; }
            [DataMember]
            public string userName { get; set; }
            [DataMember]
            public string roleName { get; set; }
            [DataMember]
            public DateTime? entry { get; set; }
            [DataMember]
            public DateTime? exits { get; set; }
        public class listHistoryLog : List<historyLogIn> { }
    }
}
