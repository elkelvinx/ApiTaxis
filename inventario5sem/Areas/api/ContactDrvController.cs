using Entidades;
using Entidades.DriversCarpet;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TaxistTeodoro.Areas.api
{
    public class ContactDriverController : ApiController
    {
        // GET: api/Admin
        
        public HttpResponseMessage Get()    
        {
            var entidad = new listContactDriver();
            var srv = new ServicioContactDriver();
            entidad = srv.consultarContactDrivers();
            var response = Request.CreateResponse<IEnumerable<contactDriver>>(System.Net.HttpStatusCode.Created, entidad);
            return response;
        }
        
        // GET: api/Admin/5
        public HttpResponseMessage Get(int id)
        {
            var entidad = new contactDriver();
            var srv = new ServicioContactDriver();
            entidad = srv.consultarContactDriver(id);
            var response = Request.CreateResponse<contactDriver>(System.Net.HttpStatusCode.Created, entidad);
            return response;
        }

        // POST: api/Admin

     
        public string Post(contactDriver obj)
        {
            var srv = new ServicioContactDriver();
            string respuesta = "ok";
            try
            {
                respuesta = srv.insertar(obj);
            }
            catch(Exception ex) { respuesta = ex.ToString(); }
           
            
            return respuesta;
        }


        // PUT: api/Admin/5
        public string Put(contactDriver obj)
        {
            var srv = new ServicioContactDriver();
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
            var srv = new ServicioContactDriver();
            try
            {
                srv.eliminar(id);
            }
            catch(Exception ex) { res = ex.ToString(); }
            return res;
        }

        /*
         * INSERTAR
     public string Post(listarAdmins ent)
     {
         var srv = new ServiciosAdmin();
         string respuesta = "ok";
         foreach (var item in ent)
         {
             respuesta = srv.insertar(item);
         }
         return respuesta;
     }
        ACTUALIZAR
           public string Put(listarAdmins value)
        {
            var srv = new ServiciosAdmin();
            string respuesta = "ok";
            foreach (var item in value)
            {
                respuesta = srv.Actualizar(item);
            }
            return respuesta;
        }

         public void Delete(int[] id)
        {
            var srv = new ServiciosAdmin();
            foreach (var item in id)
            {
                srv.eliminar(item);
            }
        }
     */
    }
}
