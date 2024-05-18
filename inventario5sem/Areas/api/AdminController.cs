using Entidades;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TaxistTeodoro.Areas.api
{
    public class AdminController : ApiController
    {
        // GET: api/Admin
        
        public HttpResponseMessage Get()    
        {
            var entidad = new listAdmin();
            var srv = new ServicioAdmin();
            entidad = srv.consultarAdmins();
            var response = Request.CreateResponse<IEnumerable<Admin>>(System.Net.HttpStatusCode.Created, entidad);
            return response;
        }
        public HttpResponseMessage Get(string val)
        {
            var entidad = new listAdmin();
            ServicioAdmin srv = new ServicioAdmin();
            if (val.Equals("n"))
            {
                entidad = srv.consultarNombre();
            }

            var response = Request.CreateResponse<IEnumerable<Admin>>(System.Net.HttpStatusCode.Created, entidad);
            return response;

        }
        // GET: api/Admin/5
        public HttpResponseMessage Get(int id)
        {
            var entidad = new Admin();
            var srv = new ServicioAdmin();
            entidad = srv.consultarAdmin(id);
            var response = Request.CreateResponse<Admin>(System.Net.HttpStatusCode.Created, entidad);
            return response;
        }

        // POST: api/Admin

     
        public string Post(Admin ent)
        {
            var srv = new ServicioAdmin();
            string respuesta = "ok";
            try
            {
                respuesta = srv.insertar(ent);
            }
            catch(Exception ex) { respuesta = ex.ToString(); }
           
            
            return respuesta;
        }


        // PUT: api/Admin/5
        public string Put(Admin Admin)
        {
            var srv = new ServicioAdmin();
            string res = "ok";
            try 
            {
                res = srv.Actualizar(Admin);
            }
            catch (Exception ex) { res = ex.ToString(); }
            return res;
        }

        // DELETE: api/Admin/5
        public string Delete(int id)
        {
            var res = "ok";
            var srv = new ServicioAdmin();
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
