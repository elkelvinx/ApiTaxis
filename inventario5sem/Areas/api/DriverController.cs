using Entidades.DriversCarpet;
using Entidades.Response;
using Servicios.DriverServices;
using Servicios.Logs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace TaxistTeodoro.Areas.api
{
    [Authorize]
    public class DriverController : ApiController
    {
        private readonly ServicioDriver _servicioDriver;
        private readonly ServicioStatsDriver _servicioStatsDriver;
        public DriverController()
        {
            _servicioDriver = new ServicioDriver();
            _servicioStatsDriver = new ServicioStatsDriver();
        }
        // GET: api/Cliente

        public HttpResponseMessage Get()
        {
            _ = new listDrivers();
            listDrivers entidad = _servicioDriver.ConsultarDrivers();
            var response = Request.CreateResponse<IEnumerable<Driver>>(System.Net.HttpStatusCode.OK, entidad);
            return response;
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get(int id)
        {
            var entidad = new Driver();
            entidad = _servicioDriver.ConsultarDriver(id);
            var response = Request.CreateResponse<Driver>(System.Net.HttpStatusCode.Created, entidad);
            return response;
        }
        //[HttpPost]
        public IHttpActionResult Grabar(Driver obj)
        {
            var response = new ApiResponse<string>();
            try
            {
                string result = _servicioDriver.Insertar(obj);
                response.Success = true;
                response.Data = result;
                //metodo para actualizar la tabla contadora total de drivers
                _servicioStatsDriver.ActualizarDriversStatusMensual(obj.hireDate);
                return Content(HttpStatusCode.Created, obj);

            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                // Captura el mensaje de error de SQL Server
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                // Maneja otras excepciones
                return Content(HttpStatusCode.InternalServerError, ex.Message);
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
                srv.Eliminar(id);
                response.Success = true;
                var hireDate = _servicioDriver.ObtenerHireDate(id);
                if (hireDate.HasValue)
                {
                    _servicioStatsDriver.DisminuirDriversMonthlyStats(hireDate.Value);
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
        //este no sirve ya al parecer
        public void reduceCountTableDriversTotal(Driver obj)
        {
            try
            {
                if (obj != null)
                    _servicioStatsDriver.DisminuirDriversMonthlyStats(obj.hireDate);
                else throw new Exception();
            }
            catch (Exception ex)
            {
                ServicioErrorLogs.RegisterErrorLogSinCMD("Settlement", 1, "UPDATE DriversMonthlyStats SET totalDrivers = totalDrivers - 1 WHERE (ingressYear * 100 + ingressMonth) = "+obj.hireDate + " AND totalDrivers > 0;"+ex, "Se borro un driver y al momento de actualizar la tabla de conteo totales de drivers fallo");
            }


        }
    }
}

