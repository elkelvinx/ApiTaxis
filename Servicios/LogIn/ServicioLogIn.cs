using Entidades.Arrays;
using Entidades.LogIn;
using Entidades.Response;
using Microsoft.IdentityModel.Tokens;
using ServicioEncriptacion;
using Servicios.Logs;
using System;
using System.Configuration;
using System.Data.SqlClient;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;
using SqlDataReader = Microsoft.Data.SqlClient.SqlDataReader;
using System.IdentityModel.Tokens.Jwt;
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
            if (Response.Success == true)
            {
                try
                {
                    _userData = (UserData)Response.Data;
                    //Aqui se encargara de registrar el history logIn
                    var JWT = GetJWT(_userData.User, _userData.Permissions);
                    ServicioHistoryLogIn.registerLogIn(JWT);
                    return new RespuestaJWT { Token = JWT };
                }
                catch (Exception ex)
                {
                    return new RespuestaJWT { ErrorMessage = "Error al intentar hacer el JWT, contacte con sistemas. " + ex };
                }
            }
            else
                return new RespuestaJWT { ErrorMessage = Response.ErrorMessage };
        }
        private ApiResponse<UserData> ValidationUser(AuthRequest Model)
        {
            var query = "SELECT d.*, p.* FROM usersData AS d " +
                        "INNER JOIN userPermissions AS p ON d.id = p.idUser " +
                        "WHERE d.name = @name AND d.password = @password ";
            var response = new ApiResponse<UserData>
            {
                Data = new UserData
                {
                    User = new User(),
                    Permissions = new UserPermissions()
                }
            };
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
                                id = reader.GetInt32(0),
                                name = reader["name"].ToString(),
                                email = reader["email"].ToString(),
                                dateCreated = DateTime.Parse(reader["dateCreated"].ToString()),
                                active = Boolean.Parse(reader["active"].ToString()),
                                bloqued = Boolean.Parse(reader["bloqued"].ToString())
                            };
                            UserPermissions permissions = new UserPermissions
                            {
                                idRole = Int16.Parse(reader["idRole"].ToString()),
                                driver = Convert.ToBoolean(reader["driver"]),
                                admin = Convert.ToBoolean(reader["admin"]),
                                permissionaire = Convert.ToBoolean(reader["permissionair"]),
                                unit = Convert.ToBoolean(reader["unit"]),
                                sinister = Convert.ToBoolean(reader["sinister"]),
                                extraData = Convert.ToBoolean(reader["extraData"]),
                                pdf = Convert.ToBoolean(reader["pdf"]),
                                changeLog = Convert.ToBoolean(reader["changeLog"])
                            };
                            reader.Close();
                            if (user.active == true)
                            {
                                response.Data.User = user;
                                response.Data.Permissions = permissions;
                                response.Success = true;
                                return response;
                            }
                            else
                            {
                                response.ErrorMessage = "El usuario actualmente esta desactivado, contacte con sistemas";
                                return response;
                            }
                        }
                    }
                }
            }
            response.ErrorMessage = "Usuario o contraseña invalidos";
            return response;
        }
        private AuthRequest EncriptData(AuthRequest Model)
        {
            Model.name = _encriptacion.SanitizeUserName(Model.name);
            Model.password = _encriptacion.GetSHA256(Model.password);
            return Model;
        }
        public string GetJWT(User user, UserPermissions perm)
        {
            var roleName = GetRole(perm.idRole);
            var key = ConfigurationManager.AppSettings["Jwt:Key"];
            var issuer = ConfigurationManager.AppSettings["Jwt:Issuer"];
            var audience = ConfigurationManager.AppSettings["Jwt:Audience"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
           {
                new Claim(ClaimTypes.NameIdentifier, user.name),
                new Claim("idUser",user.id.ToString()),
                new Claim(ClaimTypes.Role,roleName),
                new Claim("active", user.active.ToString()),
                new Claim("bloqued", user.bloqued.ToString()),
                new Claim("Driver",perm.driver.ToString()),
                new Claim("Admin",perm.admin.ToString()),
                new Claim("Permissionaire",perm.permissionaire.ToString()),
                new Claim("Unit",perm.unit.ToString()),
                new Claim("Sinister",perm.sinister.ToString()),
                new Claim("ExtraData",perm.extraData.ToString()),
                new Claim("Logs", perm.changeLog.ToString()),
                new Claim("PDF",perm.pdf.ToString()),

            };
            //Crear el token
            var claimsIdentity = new ClaimsIdentity(claims, "CustomAuthenticationClaim");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audience,
                issuer: issuer,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials);
            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtTokenString;
        }
        public String GetRole(int idUser)
        {
            switch (idUser)
            {
                case 1:
                    return "admin";
                case 2:
                    return "guest";
                case 3:
                    return "user";
            }
            return "";
        }
        public bool closeSession(string nameUser)
        {
            bool response = false;
            string cadena = "update historyLogIn set exits = @exits where userName = @userName and exits is null";
            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(cadena, con))
                {
                    cmd.Parameters.AddWithValue("@exits", DateTime.Now);
                    cmd.Parameters.AddWithValue("@userName", nameUser);
                    try
                    {
                        //con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        response = rowsAffected > 0;
                    }
                    catch (Exception e)
                    {
                        con.Close();
                        ServicioErrorLogs.RegisterErrorLog("HistoryLogIn", 69, cmd, e.Message);
                    }
                    finally { con.Close(); }
                }
            }
            return response;
        }
    }
}

