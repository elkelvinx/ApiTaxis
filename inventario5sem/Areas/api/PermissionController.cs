using Entidades;
using Servicios;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace TaxistTeodoro.Areas.api
{
    public class PermissionController : ApiController
    {
        // GET: api/Cliente
       
        public HttpResponseMessage Get()
        {
            var entidad = new listPermission();
            ServicioPermission srv = new ServicioPermission();
            entidad = srv.consultarPermissions();
            var response = Request.CreateResponse<IEnumerable<Permission>>(System.Net.HttpStatusCode.Created, entidad);
            return response;
        }
        
        // GET: api/Cliente/5
        public HttpResponseMessage Get(int id)
        {
            var entidad = new Permission();
            ServicioPermission srv = new ServicioPermission();
            entidad = srv.consultarPermissionaire(id);
            var response = Request.CreateResponse<Permission>(System.Net.HttpStatusCode.Created, entidad);
            return response;
        }

        // POST: api/Cliente
        public string Post(Permission obj)
        {
            var res = "ok";
            var srv = new ServicioPermission();
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
        public string Put(Permission obj)
        {
            var res = "ok";
            var srv = new ServicioPermission();
            try
            {
                res = srv.Actualizar(obj);
            }
            catch (Exception ex) { res = ex.ToString(); }
            return res;
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
        public void Delete(int id)
        {
            var srv = new ServicioPermission();
            srv.eliminar(id);
        }
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
