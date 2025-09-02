using Entidades.Arrays;
using Entidades.Logs;
using System;
using System.Collections.Generic;
using System.Data;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using static Entidades.Logs.historyLogIn;
using Entidades.Response;

namespace Servicios.Logs
{
    public class ServicioChangeLog
    {
        public static string UpdateTriggerChangeLog(string nameTable, int DML, SqlCommand query)
        {
            var obj = new ChangeLog();
            var response = "ok";
            string cadena = "insert into changeLog (userName,idUser,roleName,nameTable,modDate,DML,query) Values (@userName,@idUser,@roleName," +
                "@nameTable,@modDate,@DML,@query)";
            //Obtencion de los claims necesarios
            ClaimsPrincipal claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
            if (claimsPrincipal != null)
            {
                obj.userName = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
                obj.roleName = claimsPrincipal.FindFirst(ClaimTypes.Role).Value;
                obj.idUser = Int16.Parse(claimsPrincipal.FindFirst("idUser").Value);
            }
            //Obtencion query ejecutado y formateo
            string queryCompleto = query.CommandText;
            foreach (SqlParameter param in query.Parameters)
            {
                try
                {
                    queryCompleto = queryCompleto.Replace(param.ParameterName, "\"" + param.Value.ToString() + "\"");
                }
                catch (Exception)
                {
                    queryCompleto = queryCompleto.Replace(param.ParameterName, "Error");
                    DML = 999; //SI DML = 999 hay un error
                }
            }
            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(cadena, con))
                {
                    cmd.Parameters.AddWithValue("@userName", obj.userName);
                    cmd.Parameters.AddWithValue("@idUser", obj.idUser);
                    cmd.Parameters.AddWithValue("@roleName", obj.roleName);
                    cmd.Parameters.AddWithValue("@nameTable", nameTable);
                    cmd.Parameters.AddWithValue("@modDate", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@DML", DML);
                    cmd.Parameters.AddWithValue("@query", queryCompleto);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception e) { return response = e.Message; }

                    return response;
                }

            }
        }
        public ApiResponse<listChangeLogs> GetAllViejo()
        {
            var data = new ApiResponse<listChangeLogs>();
            var list = new listChangeLogs();
            string cadena = "select * from changeLog";
            try
            {
                using (SqlConnection con = ServiciosBD.ObtenerConexion())
                {
                    con.Open();
                    using (var command = new SqlCommand(cadena, con))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var obj = new ChangeLog
                            {

                                id = Convert.ToInt32(reader["id"]),
                                idUser = Convert.ToInt32(reader["idUser"]),
                                userName = reader["userName"].ToString(),
                                roleName = reader["roleName"].ToString(),
                                nameTable = reader["nameTable"].ToString(),
                                modDate = DateTime.Parse(reader["modDate"].ToString()),
                                DML = Convert.ToInt32(reader["DML"].ToString()),
                                query = reader["query"].ToString(),
                            };
                            list.Add(obj);
                        }
                    }
                    SqlConnection.ClearPool(con);
                    con.Close();
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

        public ApiResponse<listChangeLogs> GetAllNuevo()
        {
            var data = new ApiResponse <listChangeLogs>();
            var list = new listChangeLogs();
            string cadena = "SELECT * FROM changeLog";
            try
            {
                using (SqlConnection con = ServiciosBD.ObtenerConexion())
                {
                    using (var command = new SqlCommand(cadena, con))
                    using (var reader = command.ExecuteReader())
                    {
                        int idOrdinal = reader.GetOrdinal("id");
                        int idUserOrdinal = reader.GetOrdinal("idUser");
                        int userNameOrdinal = reader.GetOrdinal("userName");
                        int roleNameOrdinal = reader.GetOrdinal("roleName");
                        int nameTableOrdinal = reader.GetOrdinal("nameTable");
                        int modDateOrdinal = reader.GetOrdinal("modDate");
                        int DMLOrdinal = reader.GetOrdinal("DML");
                        int query = reader.GetOrdinal("query");

                        while (reader.Read())
                        {
                            var obj = new ChangeLog
                            {
                                id = reader.GetInt32(idOrdinal),
                                idUser = reader.GetInt32(idUserOrdinal),
                                userName = reader.GetString(userNameOrdinal),
                                roleName = reader.GetString(roleNameOrdinal),
                                nameTable = reader.GetString(nameTableOrdinal),
                                modDate = reader.GetDateTime(modDateOrdinal),
                                DML = reader.GetInt32(DMLOrdinal),
                                query = reader.GetString(query),
                            };
                            list.Add(obj);
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
        public async Task<ApiResponse<List<ChangeLog>>> GetAllAsync()
        {
            var data = new ApiResponse<List<ChangeLog>>();
            var list = new List<ChangeLog>();
            string cadena = "SELECT * FROM changeLog";

            try
            {
                using (SqlConnection con = ServiciosBD.ObtenerConexion())
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync();
                    }
                    using (var command = new SqlCommand(cadena, con))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var obj = new ChangeLog
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                idUser = reader.GetInt32(reader.GetOrdinal("idUser")),
                                userName = reader.GetString(reader.GetOrdinal("userName")),
                                roleName = reader.GetString(reader.GetOrdinal("roleName")),
                                nameTable = reader.GetString(reader.GetOrdinal("nameTable")),
                                modDate = reader.GetDateTime(reader.GetOrdinal("modDate")),
                                DML = reader.GetInt32(reader.GetOrdinal("DML")),
                                query = reader.GetString(reader.GetOrdinal("query"))
                            };
                            list.Add(obj);
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