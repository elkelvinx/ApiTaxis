using Entidades.Logs;
using System;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Servicios.Logs
{
    public class ServicioHistoryLogIn
    {
        public static void registerLogIn(string JWT)
        {
            var obj = new historyLogIn();
            obj.idUser = 0;
            string cadena = "insert into historyLogIn" +
                "(idUser,userName,roleName,entry) " +
                "VALUES(@idUser,@userName,@roleName,@entry)";
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(JWT) as JwtSecurityToken;
            if (jsonToken != null)
            {
                var claims = jsonToken.Claims;
                obj.userName = claims.First(claim => claim.Type == "nameid").Value;
                obj.roleName = claims.First(claim => claim.Type == "role").Value;
                obj.idUser = Int16.Parse(claims.First(claim => claim.Type == "idUser").Value);
            }
            verifySessionState(obj.idUser);
            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(cadena, con))
                {
                    cmd.Parameters.AddWithValue("@userName", obj.userName);
                    cmd.Parameters.AddWithValue("@idUser", obj.idUser);
                    cmd.Parameters.AddWithValue("@roleName", obj.roleName);
                    cmd.Parameters.AddWithValue("@entry", DateTime.Now);
                    //cmd.Parameters.AddWithValue("@exits", DateTime.UtcNow);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception e)
                    {
                        ServicioErrorLogs.RegisterErrorLog("HistoryLogIn", 69, cmd, e.Message);
                    }
                }

            }
        }
        public static bool verifySessionState(int idUser)
        {
            bool response = true;
            string cadena = "select userName from historyLogIn where idUser=@idUser and exits is NULL";
            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(cadena, con))
                {
                    cmd.Parameters.AddWithValue("@idUser", idUser);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Close();
                            string updateQuery = "update historyLogIn set exits = @exits where idUser = @idUser and exits is null";
                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, con))
                            {
                                DateTime localTime = DateTime.UtcNow.ToLocalTime();
                                updateCmd.Parameters.AddWithValue("@exits", DateTime.Now);
                                updateCmd.Parameters.AddWithValue("@idUser", idUser);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else reader.Close();
                    }
                }

            }
            return response;
        }
    }
}
