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
    public class AuthRequest
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string password { get; set; }
    }
}
