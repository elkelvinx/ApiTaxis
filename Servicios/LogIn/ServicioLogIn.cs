using Entidades.LogIn;
using Entidades.Response;
using Microsoft.IdentityModel.Tokens;
using ServicioEncriptacion;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens.Configuration;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace Servicios
{

    public class ServicioLogIn
    {
        public AppDomainInitializer AppDomainInitializer { get; set; }
        private readonly Encriptacion _encriptacion;
        private UserData _userData;

        public ServicioLogIn()
        {
            _encriptacion = new Encriptacion();
            _userData = new UserData();
        }
        public RespuestaJWT LogIn(AuthRequest Model)
        {
            Model = EncriptData(Model);
            var Response = ValidationUser(Model);
            if (Response.IsSuccess == true)
            {
                _userData = (UserData)Response.Data;
                var JWT = GetJWT(_userData.User, _userData.Permissions);
                return new RespuestaJWT { Token = JWT };
            }
            else
                //return new RespuestaJWT { ErrorMessage = "Nombre de usuario Invalido" };
                return null;
        }
        private RespuestaObj ValidationUser(AuthRequest Model)
        {
            var query = "SELECT d.*, p.* FROM usersData AS d " +
                        "INNER JOIN userPermissions AS p ON d.id = p.idUser " +
                        "WHERE d.name = @name AND d.password = @password";

            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@name", Model.name);
                    cmd.Parameters.AddWithValue("@password", Model.password);
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
                                idRole = Int16.Parse(reader["idRole"].ToString()),
                                driver= Convert.ToBoolean(reader["driver"]),
                                admin = Convert.ToBoolean(reader["admin"]),
                                permissionaire = Convert.ToBoolean(reader["permissionair"]),
                                unit = Convert.ToBoolean(reader["unit"]),
                                sinister = Convert.ToBoolean(reader["sinister"]),
                                extraData = Convert.ToBoolean(reader["extraData"]),
                                pdf = Convert.ToBoolean(reader["pdf"])
                            };
                            reader.Close();
                            return new RespuestaObj
                            {
                                Data = new UserData { User = user, Permissions = permissions }
                            };
                        }
                    }
                }
            }
            return new RespuestaObj { ErrorMessage = "Fallo en el proceso" };
        }
        private AuthRequest EncriptData(AuthRequest Model)
        {
            Model.name = _encriptacion.SanitizeUserName(Model.name);
            Model.password = _encriptacion.GetSHA256(Model.password);
            return Model;
        }
        public string GetJWT(User user, UserPermissions perm)
        {
          
            var key = ConfigurationManager.AppSettings["Jwt:Key"];
            var issuer = ConfigurationManager.AppSettings["Jwt:Issuer"];
            var audience = ConfigurationManager.AppSettings["Jwt:Audience"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
           {
                new Claim(ClaimTypes.NameIdentifier, user.name),
                new Claim(ClaimTypes.Email, user.email),
                new Claim(ClaimTypes.Role, perm.idRole.ToString()),
                new Claim("Driver",perm.driver.ToString()),
                new Claim("Admin",perm.admin.ToString()),
                new Claim("Permissionaire",perm.permissionaire.ToString()),
                new Claim("Unit",perm.unit.ToString()),
                new Claim("Sinister",perm.sinister.ToString()),
                new Claim("ExtraData",perm.extraData.ToString()),
                new Claim("PDF",perm.pdf.ToString()),
                
            };

            // Crear el token            
              var token = new JwtSecurityToken(
                 issuer,
                 audience,
                 claims,
                 expires: DateTime.Now.AddMinutes(60),
                 signingCredentials: credentials);

             return new JwtSecurityTokenHandler().WriteToken(token); 
        }
        public String GetRole(int idUser)
        {
            switch (idUser) {
                case 1:
                    return "Admin";
                case 2:
                    return "Guest";
                case 3:
                    return "User";                   
            }
            return "";
        }
    }
}

