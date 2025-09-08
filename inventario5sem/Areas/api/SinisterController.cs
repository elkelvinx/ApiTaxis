using Entidades.Cars;
using Entidades.Response;
using Microsoft.Data.SqlClient;
using Servicios;
using Servicios.UnitsCarpet;
using Servicios.Logs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TaxistTeodoro.Areas.api
{
    //[Authorize]
    public class SinisterController : ApiController
    {
        private readonly ServicioSinister _servicioSinister;
        private readonly ServicioStatsSinister _servicioStatsSinister;

        public SinisterController()
        {
            _servicioSinister = new ServicioSinister();
            _servicioStatsSinister = new ServicioStatsSinister();
        }

        [HttpGet]
        [Route("api/Sinister")]
        public HttpResponseMessage Get()
        {
            var entidad = _servicioSinister.consultarSiniestros();
            var response = Request.CreateResponse<IEnumerable<Sinister>>(HttpStatusCode.OK, entidad);
            return response;
        }

        [HttpGet]
        [Route("api/Sinister/{id}")]
        public HttpResponseMessage Get(int id)
        {
            var entidad = _servicioSinister.consultarSiniestro(id);
            var response = Request.CreateResponse<Sinister>(HttpStatusCode.OK, entidad);
            return response;
        }

        [HttpPost]
        [Route("api/Sinister")]
        public IHttpActionResult Post(Sinister obj)
        {
            var response = new ApiResponse<string>();
            try
            {
                string result = _servicioSinister.insertar(obj);
                response.Success = true;
                response.Data = result;

                // ✅ actualizar tabla KPI siniestros
                
                 _servicioStatsSinister.ActualizarSinisterStatusMensual(obj.dateEvent);
                

                return Content(HttpStatusCode.Created, obj);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("api/Sinister")]
        public IHttpActionResult Put(Sinister obj)
        {
            var response = new ApiResponse<string>();
            try
            {
                string result = _servicioSinister.Actualizar(obj);
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

        [HttpDelete]
        [Route("api/Sinister/{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = new ApiResponse<string>();
            try
            {
                var sinister = _servicioSinister.consultarSiniestro(id);
                _servicioSinister.eliminar(id);
                response.Success = true;

                if (sinister != null)
                {
                    _servicioStatsSinister.DisminuirSinisterMonthlyStats(sinister.dateEvent);
                }

                return Ok(response);
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

        // Este helper sería como el de drivers, por si quieres invocarlo manualmente
        [NonAction]
        public void reduceCountTableSinistersTotal(Sinister obj)
        {
            try
            {
                if (obj != null)
                    _servicioStatsSinister.DisminuirSinisterMonthlyStats(obj.dateEvent);
                else throw new Exception();
            }
            catch (Exception ex)
            {
                ServicioErrorLogs.RegisterErrorLogSinCMD(
                    "Sinister",
                    1,
                    "UPDATE SinistersMonthlyStats SET totalSinisters = totalSinisters - 1 " +
                    "WHERE (eventYear * 100 + eventMonth) = " + obj?.dateEvent +
                    " AND totalSinisters > 0;" + ex,
                    "Se borró un siniestro y al actualizar la tabla de conteo totales de siniestros falló"
                );
            }
        }
    }
}
