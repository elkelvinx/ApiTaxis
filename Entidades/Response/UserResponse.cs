using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Response
{
    public class UserResponse
    {
        public string email {  get; set; }
        public int role { get; set; }
        public string Token { get; set; }
    }
}
