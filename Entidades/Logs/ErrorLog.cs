using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace Entidades.Logs
{
    [Serializable]
    [DataContract]
    public class ErrorLog
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string userName { get; set; }
        [DataMember]
        public int idUser { get; set; }
        [DataMember]
        public string nameTable { get; set; }
        [DataMember]
        public string MessageError { get; set; }
        [DataMember]
        public DateTime? DateError { get; set; }
        [DataMember]
        public string query { get; set; }
        [DataMember]
        public int DML { get; set;}

    }
    public class listErrorLogs : List<ErrorLog> { }

}
