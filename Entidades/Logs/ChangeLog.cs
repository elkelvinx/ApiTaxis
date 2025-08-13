using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace Entidades.Logs
{
    [Serializable]
    [DataContract]
    public class ChangeLog
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string userName { get; set; }
        [DataMember]
        public int idUser { get; set; }
        [DataMember]
        public string roleName { get; set; }
        [DataMember]
        public string nameTable { get; set; }
        [DataMember]
        public DateTime modDate { get; set; }
        [DataMember]
        public int DML { get; set; }
        [DataMember]
        public string query { get; set; }

    }
    public class listChangeLogs : List<ChangeLog> { }
}
