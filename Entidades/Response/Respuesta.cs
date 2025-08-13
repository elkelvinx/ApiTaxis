using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Response
{
    public class RespuestaObj
    {
        public bool IsSuccess => Data != null && string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; set; }
        public object Data { get; set; }

    }
    public class RespuestaJWT
    {
        public bool IsSuccess => Token != null && string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
    }
    public class ResponsGeneric<T>
    {
        public bool IsSuccess => Data != null && string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }
}
