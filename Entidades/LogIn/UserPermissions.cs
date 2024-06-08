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
        public int Id { get; set; }
        [DataMember]
        public int IdUser { get; set; }
        [DataMember]
        public int IdRole { get; set; }
        [DataMember]
        public bool Driver { get; set; }
        [DataMember]
        public bool Admin { get; set; }
        [DataMember]
        public bool Permissionaire { get; set; }
        [DataMember]
        public bool Unit { get; set; }
        [DataMember]
        public bool Sinister { get; set; }
        [DataMember]
        public bool ExtraData { get; set; }
        [DataMember]
        public bool Pdf { get; set; }
    }
}
