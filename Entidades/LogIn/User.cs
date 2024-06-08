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
    public class User
    {
        [DataMember]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [DataMember]
        public string email { get; set; }
        [Required]
        public int password { get; set; }
        [DataMember]
        public DateTime dateCreated { get; set; }

    }
    public class UserData
    {
        public User User { get; set; }
        public UserPermissions Permissions { get; set; }
    }
}
