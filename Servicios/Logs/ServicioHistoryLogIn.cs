using Entidades.Arrays;
using Entidades.Logs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using static Entidades.Logs.historyLogIn;

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
        /*
         * una segunda opcion es que si hay una session abierta 
         * entonces la API hara uso de esa sesion para continuar funcionando
         * por lo que no la cerrrara
         * lo que haria que hayan menos entradas y salidas en la tabla
         */
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
        public ApiResponse<List<historyLogIn>> GetAll()
        {
            var data = new ApiResponse<List<historyLogIn>>();
            var list = new List<historyLogIn>();
            string cadena = "SELECT * FROM historyLogIn";

            try
            {
                using (SqlConnection con = ServiciosBD.ObtenerConexion())
                {
                    //con.Open();
                    using (var command = new SqlCommand(cadena, con))
                    using (var reader = command.ExecuteReader())
                    {
                        int idOrdinal = reader.GetOrdinal("id");
                        int idUserOrdinal = reader.GetOrdinal("idUser");
                        int userNameOrdinal = reader.GetOrdinal("userName");
                        int roleNameOrdinal = reader.GetOrdinal("roleName");
                        int entryOrdinal = reader.GetOrdinal("entry");
                        int exitsOrdinal = reader.GetOrdinal("exits");

                        while (reader.Read())
                        {
                            var logInData = new historyLogIn
                            {
                                id = reader.GetInt32(idOrdinal),
                                idUser = reader.GetInt32(idUserOrdinal),
                                userName = reader.GetString(userNameOrdinal),
                                roleName = reader.GetString(roleNameOrdinal),
                                entry = reader.IsDBNull(entryOrdinal) ? (DateTime?)null : reader.GetDateTime(entryOrdinal),
                                exits = reader.IsDBNull(exitsOrdinal) ? (DateTime?)null : reader.GetDateTime(exitsOrdinal)
                            };
                            list.Add(logInData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.ErrorMessage = ex.Message;
                data.Success = false;
                return data;
            }

            data.Data = list;
            data.Success = true;
            return data;
        }

    }
}
