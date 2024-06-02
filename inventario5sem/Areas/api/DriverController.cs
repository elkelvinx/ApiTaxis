using Entidades;
using Entidades.Arrays;
using Entidades.Response;
using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web.Http;

namespace TaxistTeodoro.Areas.api
{
    public class DriverController : ApiController
    {
        private readonly ServicioDriver _servicioDriver;
        public DriverController()
        {
            _servicioDriver = new ServicioDriver();
        }
        // GET: api/Cliente

        public HttpResponseMessage Get()
        {
            var entidad = new listDrivers();
            entidad = _servicioDriver.consultarDrivers();
            var response = Request.CreateResponse<IEnumerable<Driver>>(System.Net.HttpStatusCode.Created, entidad);
            return response;
        }
      
        // GET: api/Cliente/5
        public HttpResponseMessage Get(int id)
        {
            var entidad = new Driver();
            entidad = _servicioDriver.consultarDriver(id);
            var response = Request.CreateResponse<Driver>(System.Net.HttpStatusCode.Created, entidad);
                return response;        
        }
        [HttpPost]
        public IHttpActionResult Grabar(Driver obj)
        {
            var response = new ApiResponse<string>();
            try
            {
                string result = _servicioDriver.insertar(obj);
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
                // Captura el mensaje de error de SQL Server
                return InternalServerError(ex);
            }
            catch (Exception ex)
            {
                // Maneja otras excepciones
                return InternalServerError(ex);
            }
        }
        public IHttpActionResult Put(Driver obj)
        {
            ApiResponse<string> response = new ApiResponse<string>();
            try
            {
                string result = _servicioDriver.Actualizar(obj);
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
        public IHttpActionResult Delete(int id)
        {
            ApiResponse<string> response = new ApiResponse<string>();
            var srv = new ServicioDriver();
            try
            {
                srv.eliminar(id);
                response.Success = true;
                return Ok(response);
            }

            catch(SqlException ex)
            {
                return InternalServerError(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

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
