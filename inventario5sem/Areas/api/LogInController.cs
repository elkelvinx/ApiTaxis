using Entidades.LogIn;
using Servicios;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Net;
using System.Web.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
namespace TaxisTeodoro.Areas.api
{
    public class LogInController : ApiController

    {
        private readonly ServicioLogIn _ServicioLogIn; // Inject Encriptacion service
        private readonly ServicioUser _ServicioUser;
        public LogInController()
        {
            _ServicioLogIn = new ServicioLogIn();
            _ServicioUser = new ServicioUser();
        }
        [Route("api/login/enter")]
        public IHttpActionResult Post([FromBody] AuthRequest Model)
        {
            //Model = _ServicioLogIn.DesEncript(Model);
            try
            {
                var response = _ServicioLogIn.LogIn(Model);
                if (response.IsSuccess == false)
                    return Content(HttpStatusCode.Unauthorized, response.ErrorMessage);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.GatewayTimeout, "Hubo un error con la base de datos, acuda con sistemas. " + ex.Message) ;
            }
        }
        public IHttpActionResult Post(UserData obj)
        {
            string respuesta = "ok";
            try
            {
                var response = _ServicioUser.Insertar(obj);
                return Ok(response + " " + obj);
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
                return Content(HttpStatusCode.BadGateway, ex.Message);

            }
        }
    }
}
