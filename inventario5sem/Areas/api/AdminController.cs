using Entidades;
using Entidades.Arrays;
using Entidades.Response;
using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TaxistTeodoro.Areas.api
{
    public class AdminController : ApiController
    {
        private readonly ServicioAdmin _servicioAdmin;
        public AdminController() { 
        _servicioAdmin = new ServicioAdmin();
        }
        
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

     
        public IHttpActionResult Post(Admin obj)
        {
            ApiResponse<string> response = new ApiResponse<string>();
            try
            {
                string result = _servicioAdmin.insertar(obj);
                response.Success = true;
                response.Data = result;
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                return InternalServerError(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // PUT: api/Admin/5
        public IHttpActionResult Put(Admin Admin)
        {
            ApiResponse<string> response = new ApiResponse<string>();
            try
            {
                string result = _servicioAdmin.Actualizar(Admin);
                response.Success = true;
                response.Data = result;
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                return InternalServerError(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE: api/Admin/5
        public IHttpActionResult Delete(int id)
        {
            ApiResponse<string> response = new ApiResponse<string>();
            try
            {
                string result = _servicioAdmin.eliminar(id);
                response.Success = true;
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                return InternalServerError(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
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
