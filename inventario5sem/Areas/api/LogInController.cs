using Entidades.LogIn;
using Servicios;
using System;
using System.ComponentModel.DataAnnotations;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;
using SqlDataReader = Microsoft.Data.SqlClient.SqlDataReader;
using System.Net;
using System.Web.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Servicios.Logs;

namespace TaxisTeodoro.Areas.api
{
   // [Authorize]
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
        [AllowAnonymous]
        public IHttpActionResult Post([FromBody] AuthRequest Model)
        {
           // Model = _ServicioLogIn.DesEncript(Model);
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
        [Route("api/login")]
        public IHttpActionResult Post([FromBody] dynamic data)
        {
            string nameUser = data.nameUser;
            string respuesta = "ok";
            try
            {
                var response = _ServicioLogIn.closeSession(nameUser);
                if (response)
                {
                    return Ok(nameUser);
                }                
                else return BadRequest("Fallo algo internamente");
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
                return Content(HttpStatusCode.BadGateway, ex.Message);

            }
        }
    }
}
