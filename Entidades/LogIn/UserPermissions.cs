using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.LogIn
{
    [Serializable]
    [DataContract]
    public class UserPermissions
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int idUser { get; set; }
        [DataMember]
        public int idRole { get; set; }
        [DataMember]
        public bool driver { get; set; }
        [DataMember]
        public bool admin { get; set; }
        [DataMember]
        public bool permissionaire { get; set; }
        [DataMember]
        public bool unit { get; set; }
        [DataMember]
        public bool sinister { get; set; }
        [DataMember]
        public bool extraData { get; set; }
        [DataMember]
        public bool pdf { get; set; }
    }
}
