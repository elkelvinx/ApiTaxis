
using Entidades.LogIn;
using ServicioEncriptacion;
using Servicios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace TaxisTeodoro.Areas.api
{
    public class LogInController : ApiController
        
    {
        private readonly Encriptacion _encriptacion; // Inject Encriptacion service
       // private readonly IAntiXss _antiXss; // Inject AntiXss service
        public LogInController()
        {
            _encriptacion = new Encriptacion();             
        }
        [Route("api/login/entrar")]
        public IHttpActionResult Post([FromBody] AuthRequest Model)
        {
            Model.name = _encriptacion.SanitizeUserName(Model.name);
            Model.password = _encriptacion.GetSHA256(Model.password);
          string query= "SELECT d.*, p.* FROM usersData AS d INNER JOIN userPermissions AS p ON d.id = p.idUser WHERE d.name = @name AND d.password = @password";
            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@name", Model.name);
                    cmd.Parameters.AddWithValue("@password", Model.password);

                    //con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User user = new User
                            {                                
                                name = reader["name"].ToString(),
                                email = reader["email"].ToString(),
                                dateCreated = DateTime.Parse(reader["dateCreated"].ToString()),                                
                            };
                            UserPermissions permissions = new UserPermissions
                            {
                                idRole= Int16.Parse(reader["idRole"].ToString()),
                                admin = Convert.ToBoolean(reader["admin"]),
                                permissionaire = Convert.ToBoolean(reader["permissionair"]),
                                unit = Convert.ToBoolean(reader["unit"]),
                                sinister = Convert.ToBoolean(reader["sinister"]),
                                extraData = Convert.ToBoolean(reader["extraData"]),
                                pdf= Convert.ToBoolean(reader["pdf"])                               
                            };
                            reader.Close();
                            return Ok("LO LOGREEE " + user.name + " "+user.email+" "+ user.id + " informacion del permiso:" + permissions.id + " " + permissions.driver);
                        }
                    }

                }
            }
                return Ok(Model);
        }
    
    }
}
