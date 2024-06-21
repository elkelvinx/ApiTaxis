using Entidades.LogIn;
using Servicios;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Net;
using System.Web.Http;
using System.IdentityModel.Tokens.Jwt;


namespace TaxisTeodoro.Areas.api
{
    public class LogInController : ApiController

    {
        private readonly ServicioLogIn _ServicioLogIn; // Inject Encriptacion service
        public LogInController()
        {
            _ServicioLogIn = new ServicioLogIn();
        }
        [Route("api/login/enter")]
        public IHttpActionResult Post([FromBody] AuthRequest Model)
        {
            //Model = _ServicioLogIn.DesEncript(Model);
            try
            {
                var response = _ServicioLogIn.LogIn(Model);
                if (response == null)
                    return Content(HttpStatusCode.Unauthorized, "Usuario o contraseña inválido");
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.GatewayTimeout, "Hubo un error con la base de datos, acuda con sistemas");
            }
        }
    }
}
