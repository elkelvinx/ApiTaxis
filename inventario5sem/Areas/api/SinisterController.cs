using Entidades;
using Entidades.cars;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TaxistTeodoro.Areas.api
{
    public class SinisterController : ApiController
    {
        // GET: api/Admin
        
        public HttpResponseMessage Get()    
        {
            var entidad = new listSinister();
            var srv = new ServicioSinister();
            entidad = srv.consultarSiniestros();
            var response = Request.CreateResponse<IEnumerable<Sinister>>(System.Net.HttpStatusCode.Created, entidad);
            return response;
        }
        
        // GET: api/Admin/5
        public HttpResponseMessage Get(int id)
        {
            var entidad = new Sinister();
            var srv = new ServicioSinister();
            entidad = srv.consultarSiniestro(id);
            var response = Request.CreateResponse<Sinister>(System.Net.HttpStatusCode.Created, entidad);
            return response;
        }

        // POST: api/Admin
     
        public string Post(Sinister ent)
        {
            var srv = new ServicioSinister();
            string respuesta = "ok";
            try
            {
                respuesta = srv.insertar(ent);
            }
            catch(Exception ex) { respuesta = ex.ToString(); }  
            return respuesta;
        }

        // PUT: api/Admin/5
        public string Put(Sinister obj)
        {
            var srv = new ServicioSinister();
            string res = "ok";
            try 
            {
                res = srv.Actualizar(obj);
            }
            catch (Exception ex) { res = ex.ToString(); }
            return res;
        }

        // DELETE: api/Admin/5
        public string Delete(int id)
        {
            var res = "ok";
            var srv = new ServicioSinister();
            try
            {
                srv.eliminar(id);
            }
            catch(Exception ex) { res = ex.ToString(); }
            return res;
        }

    }
}
