using Entidades.Logs;
using System;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Threading;

namespace Servicios.Logs
{
    public class ServicioErrorLogs
    {
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
                obj.UserName = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
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
                    cmd.Parameters.AddWithValue("@userName", obj.UserName);
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
    }
}
