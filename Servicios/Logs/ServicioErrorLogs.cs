using Entidades.Arrays;
using Entidades.Logs;
using Entidades.Response;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using System.Threading;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;
using SqlDataReader = Microsoft.Data.SqlClient.SqlDataReader;

namespace Servicios.Logs
{
    public class ServicioErrorLogs
    {
        //hacer otro metodo que no pida una query, solo una descripcion detallada
        public static string RegisterErrorLog(string NameTable, int DML, SqlCommand Query, string MessageError)
        {
            var obj = new ErrorLog();
            var response = "ok";
            string cadena = "insert into errorLogs (userName,idUser,nameTable,messageError,dateError,query,DML) Values (@userName,@idUser,@nameTable," +
                "@messageError,@dateError,@query,@DML)";
            //Obtencion de los claims necesarios
            ClaimsPrincipal claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
            if (claimsPrincipal != null)
            {
                obj.userName = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
                obj.idUser = Int16.Parse(claimsPrincipal.FindFirst("idUser").Value);
            }
            //Obtencion query ejecutado y formateo
            string queryCompleto = Query.CommandText;
            foreach (SqlParameter param in Query.Parameters)
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
                    cmd.Parameters.AddWithValue("@nameTable", NameTable);
                    cmd.Parameters.AddWithValue("@dateError", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@DML", DML);
                    cmd.Parameters.AddWithValue("@query", queryCompleto);
                    cmd.Parameters.AddWithValue("@messageError", MessageError);
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
        //metodo para poder ingresar cuando falla util para tablas de conteos como la de drivers usar pocas veces para no perder formato
        public static string RegisterErrorLogSinCMD(string NameTable, int DML, string Query, string MessageError)
        {
            var obj = new ErrorLog();
            var response = "ok";
            string cadena = "insert into errorLogs (userName,idUser,nameTable,messageError,dateError,query,DML) Values (@userName,@idUser,@nameTable," +
                "@messageError,@dateError,@query,@DML)";
            //Obtencion de los claims necesarios
            ClaimsPrincipal claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
            if (claimsPrincipal != null)
            {
                obj.userName = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
                obj.idUser = Int16.Parse(claimsPrincipal.FindFirst("idUser").Value);
            }
         
            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(cadena, con))
                {
                    cmd.Parameters.AddWithValue("@userName", obj.userName);
                    cmd.Parameters.AddWithValue("@idUser", obj.idUser);
                    cmd.Parameters.AddWithValue("@nameTable", NameTable);
                    cmd.Parameters.AddWithValue("@dateError", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@DML", DML);
                    cmd.Parameters.AddWithValue("@query", Query);
                    cmd.Parameters.AddWithValue("@messageError", MessageError);
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
        public ApiResponse<listErrorLogs> GetAll()
        {
            var data = new ApiResponse<listErrorLogs>();
            var list = new listErrorLogs();
            string cadena = "SELECT * FROM errorLogs";
            try
            {
                using (SqlConnection con = ServiciosBD.ObtenerConexion())
                {
                    //con.Open();
                    using (var command = new SqlCommand(cadena, con))
                    using (var reader = command.ExecuteReader())
                    {
                        int idOrdinal = reader.GetOrdinal("id");
                        int userNameOrdinal = reader.GetOrdinal("userName");
                        int idUserOrdinal = reader.GetOrdinal("idUser");
                        int nameTableOrdinal = reader.GetOrdinal("nameTable");
                        int messageErrorOrdinal = reader.GetOrdinal("messageError");
                        int query = reader.GetOrdinal("query");
                        int dateErrorOrdinal = reader.GetOrdinal("dateError");
                        int DMLOrdinal = reader.GetOrdinal("DML");

                        while (reader.Read())
                        {
                            var obj = new ErrorLog
                            {
                                id = reader.GetInt32(idOrdinal),
                                idUser = reader.GetInt32(idUserOrdinal),
                                userName= reader.GetString(userNameOrdinal),
                                nameTable = reader.GetString(nameTableOrdinal),
                                query = reader.GetString(query),
                                DateError = reader.IsDBNull(dateErrorOrdinal) ? (DateTime?)null : reader.GetDateTime(dateErrorOrdinal),
                                DML = reader.GetInt32(DMLOrdinal),
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
