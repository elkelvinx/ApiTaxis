using Entidades;
using Servicios;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace TaxistTeodoro.Areas.api
{
    public class DriverController : ApiController
    {
        // GET: api/Cliente
        
        public HttpResponseMessage Get()
        {
            var entidad = new listDrivers();
            ServicioDriver srv = new ServicioDriver();
            entidad = srv.consultarDrivers();
            var response = Request.CreateResponse<IEnumerable<Driver>>(System.Net.HttpStatusCode.Created, entidad);
            return response;
        }
      
        // GET: api/Cliente/5
        public HttpResponseMessage Get(int id)
        {
            var entidad = new Driver();
            ServicioDriver srv = new ServicioDriver();
            
            entidad = srv.consultarDriver(id);
            var response = Request.CreateResponse<Driver>(System.Net.HttpStatusCode.Created, entidad);
            return response;
        }

        // POST: api/Cliente
        public string Post(Driver obj)
        {
            var res = "ok";
            var srv = new ServicioDriver();
            try
            {
                res= srv.insertar(obj);
            }
            catch (Exception ex) {
                var error = ex.Message;
                res = error;

            }
            return res;
        }

        // PUT: api/Cliente/5
        public string Put(Driver obj)
        {
            var res = "ok";
            var srv = new ServicioDriver();
            try
            {
                res = srv.Actualizar(obj);
            }
            catch (Exception ex) { res = ex.ToString(); }
            return res;
        }
        public void Delete(int id)
        {
            var srv = new ServicioDriver();
            srv.eliminar(id);
        }

        // DELETE: api/Cliente/5
        /*
        public string Delete(int id)
        {
            string res = "ok";
            var srv = new ServicioCliente();
           try
            {
                res = srv.eliminar(id);
            }
            catch (Exception ex) { res = ex.ToString(); }

            return res;
        }
        */

        /*
         public void Post(listarCliente ent)
        {
            var srv = new ServicioCliente();
            foreach (var item in ent)
            {
                srv.insertar(item);
            }
        }
         */
    }
}
