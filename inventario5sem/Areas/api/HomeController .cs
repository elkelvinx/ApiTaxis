using Entidades;
using Servicios;
using System;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Runtime.Caching;
using System.Web.Http;

namespace TaxisTeodoro.Areas.api
{
    //[Authorize]
    [RoutePrefix("api/home")]
    public class HomeController : ApiController
    {
        private readonly ServicioHome _servicioHome;
        private static readonly MemoryCache _cache = MemoryCache.Default;
        public HomeController()
        {
            _servicioHome = new ServicioHome();
        }
        // 1) Total de conductores registrados
        [HttpGet]
        [Route("driversKpi")]
        [AllowAnonymous]
        public IHttpActionResult GetDriversCount()
        {
            try
            {
                var totalDrivers = _servicioHome.CalculateCurrentDriversKpi();
                return Ok(totalDrivers);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        // 2) KPI Siniestros
        [HttpGet]
        [Route("sinistersKpi")]
        public IHttpActionResult GetSinistersKpi()
        {
            try
            {
                var totalSinister = _servicioHome.CalculateCurrentSinistersKpi();
                return Ok(totalSinister);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        // 3) Serie Conductores por rango (para gráfica)
        [HttpPost]
        [Route("driversRange")]
        public IHttpActionResult GetDriversRange([FromBody] DateRangeDto dates)
        {
            string key = $"driversSeries-{dates?.StartDate:yyyyMMdd}-{dates?.EndDate:yyyyMMdd}";

            if (_cache.Contains(key))
                return Ok(_cache.Get(key));
            try
            {
                var list = _servicioHome.GetDriversSeries(dates?.StartDate, dates?.EndDate);
                _cache.Set(key, list, DateTimeOffset.Now.AddMinutes(0.5));
                return Ok(list);
            }
            catch (SqlException ex)
            {
                // errores de base de datos
                return Content(HttpStatusCode.ServiceUnavailable,
                    $"Error al acceder a la BD: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                // errores de parámetros
                return BadRequest($"Datos inválidos: {ex.Message}");
            }
            catch (Exception ex)
            {
                // error genérico
                return InternalServerError(new Exception("Ocurrió un error inesperado, intente más tarde."));
            }
        }
        // 4) Serie Siniestros por rango (para gráfica)
        [HttpPost]
        [Route("sinistersRange")]
        [AllowAnonymous]
        public IHttpActionResult GetSinistersRange([FromBody] DateRangeDto dates)
        {
            string key = $"sinisterSeries-{dates?.StartDate:yyyyMMdd}-{dates?.EndDate:yyyyMMdd}";

            if (_cache.Contains(key))
                return Ok(_cache.Get(key));

            try
            {
                var list = _servicioHome.GetSinistersSeries(dates?.StartDate, dates?.EndDate);
                _cache.Set(key, list, DateTimeOffset.Now.AddMinutes(0.5));
                return Ok(list);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }

}
