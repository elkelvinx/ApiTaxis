using Entidades;
using Servicios;
using System;
using System.Net;
using System.Web.Http;

namespace TaxisTeodoro.Areas.api
{
    [RoutePrefix("api/home")]
    public class HomeController : ApiController
    {
        private readonly ServicioHome _servicioHome;
        public HomeController()
        {
            _servicioHome = new ServicioHome();
        }
        // Total de conductores registrados
        [HttpGet]
        [Route("driversCount")]
        [AllowAnonymous]
        public IHttpActionResult GetDriversCount()
        {
            int totalDrivers = 0;
            try
            {
                totalDrivers = _servicioHome.GetDriversCount();
                return Content(HttpStatusCode.Created, totalDrivers);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        //Siniestros que sucedieron en el mes actual
        [HttpGet]
        [Route("SinisterMonthly")]
        [AllowAnonymous]
        public IHttpActionResult GetSinistersPerMoth()
        {
            int totalSinisters = 0;
            try
            {
                totalSinisters = _servicioHome.GetSinistersThisMonth();
                return Content(HttpStatusCode.Created, totalSinisters);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("DriverRange")]
        [AllowAnonymous]
        public IHttpActionResult GetDriversRisePerMonth(DateTime? startMonthRange = null, DateTime? endMonthRange = null)
        {
            var totalDrivers = new listHomeDriversRise();
            try
            {
                totalDrivers = _servicioHome.homeDriversRises(startMonthRange,endMonthRange);
                return Content(HttpStatusCode.OK, totalDrivers);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}
