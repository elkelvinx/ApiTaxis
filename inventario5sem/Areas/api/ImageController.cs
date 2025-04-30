using Entidades.LogIn;
using Servicios;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Net;
using System.Web.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Servicios.Logs;
using System.IO;
using System.Web;

namespace TaxisTeodoro.Areas.api
{

    public class ImageController : ApiController
    {
        private readonly ServicioLogIn _ServicioLogIn; // Inject Encriptacion service
        private readonly ServicioUser _ServicioUser;
        public ImageController()
        {
            _ServicioLogIn = new ServicioLogIn();
            _ServicioUser = new ServicioUser();
        }
        /*
        [HttpPost]
        [Route("guardarImagen")]
          public string guardarImagen()
         {
             var httpRequest = HttpContext.Current.Request;
             var postedFile = httpRequest.Files["Archivo"];

             var ruta = string.Empty;
             if (postedFile != null && postedFile.ContentLength > 0)
             {
                 var nombreArchivo = Guid.NewGuid().ToString() + ".jpg";
                 //ruta = HttpContext.Current.Server.MapPath("~/Imagenes/") + nombreArchivo;
                 ruta = @"C:\Archivos_progr-\ANGULAR\Teodoro programa\extra\estructure\Uploaded_IMG\" + nombreArchivo;
                 using (var fileStream = new FileStream(ruta, FileMode.Create))
                 {
                     postedFile.InputStream.CopyTo(fileStream);
                 }
                 // Guardar la ruta en la base de datos
                 using (var connection = new SqlConnection(connectionString))
                 {
                     var query = "INSERT INTO Imagenes (RutaImagen, CategoriaID) VALUES (@RutaImagen, @CategoriaID)";
                     var command = new SqlCommand(query, connection);
                     command.Parameters.AddWithValue("@RutaImagen", ruta);
                     command.Parameters.AddWithValue("@CategoriaID", categoriaId);
                     connection.Open();
                     command.ExecuteNonQuery();
                 }
             }
             return Ok(ruta);
         }
         */
    }
}
