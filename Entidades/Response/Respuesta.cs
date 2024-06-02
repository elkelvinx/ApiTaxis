using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Response
{
   
    public class Respuesta
    {
        public int Exito {  get; set; }
        public string Mensaje { get; set; }
        public object Data { get; set; }
        public Respuesta() {
        this.Exito = 0;
        }
    }
}
