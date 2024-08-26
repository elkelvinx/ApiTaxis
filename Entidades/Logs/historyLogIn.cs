using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Entidades.Logs
{
    public class historyLogIn
    {
        [Serializable]
        [DataContract]
        public class regtisterLogIn
        {
            [DataMember]
            public int id { get; set; }
            [DataMember]
            public int idUser { get; set; }
            [DataMember]
            public string userName { get; set; }
            [DataMember]
            public int idRole { get; set; }
            [DataMember]
            public DateTime entry { get; set; }
            [DataMember]
            public DateTime exits { get; set; }
        }
        public class listHistoryLog : List<historyLogIn> { }
    }
}
