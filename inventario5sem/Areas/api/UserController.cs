using Entidades;
using Entidades.Arrays;
using Entidades.LogIn;
using Servicios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TaxistTeodoro.Areas.api
{
    public class UserController : ApiController
    {
        private readonly ServicioUser _servicioUser;
        public UserController()
        {
            _servicioUser= new ServicioUser();
        }
        
        public HttpResponseMessage Get()    
        {
            var info = new ApiResponse<listUsersData>();
            try
            {
                info= _servicioUser.ConsultarUsers();
                if(!info.Success)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, info.ErrorMessage);
                }
                var response = Request.CreateResponse<IEnumerable<UserData>>(System.Net.HttpStatusCode.OK, info.Data);
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            } 
            
        }
        public IHttpActionResult Post(UserData obj)
        {
            ApiResponse<string> response = new ApiResponse<string>();
            var srv = new ServicioUser();
            try
            {
                response.Data = srv.Insertar(obj);
                if(!response.Success)
                {
                    return Content(HttpStatusCode.BadRequest, response);
                }
            }
            catch(Exception ex) {
                return Content(HttpStatusCode.InternalServerError, response.ErrorMessage=ex.Message);
            }
            return Content(HttpStatusCode.Created, response);
        }
        public IHttpActionResult Put(UserData obj)
        {
            ApiResponse<string> response = new ApiResponse<string>();
            var srv = new ServicioUser();
            try
            {
                response = srv.Actualizar(obj);
                if (!response.Success)
                {
                    return Content(HttpStatusCode.BadRequest, response);
                }
                else return Content(HttpStatusCode.Accepted, response);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, response.ErrorMessage = ex.Message);
            }
        }
        public IHttpActionResult Delete(int id)
        {
            ApiResponse<string> response = new ApiResponse<string>();
            var srv = new ServicioUser();
            try
            {
                srv.softDelete(id);
                if (!response.Success)
                {
                    return Content(HttpStatusCode.BadRequest, response);
                }
                else return Content(HttpStatusCode.Accepted, response);
            }
            catch(Exception ex) { 
                return Content(HttpStatusCode.InternalServerError, response.ErrorMessage = ex.Message);
            }
        }

    }
}
