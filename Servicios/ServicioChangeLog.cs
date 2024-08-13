using Entidades;
using System;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Threading;

namespace Servicios
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
                obj.UserName = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
                obj.roleName = claimsPrincipal.FindFirst(ClaimTypes.Role).Value;
                obj.idUser = Int16.Parse(claimsPrincipal.FindFirst("idUser").Value);
            }
            //Obtencion query ejecutado y formateo
            string queryCompleto = query.CommandText;
            foreach (SqlParameter param in query.Parameters)
            {
                queryCompleto = queryCompleto.Replace(param.ParameterName, "\"" + param.Value.ToString() + "\"");
            }
            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(cadena, con))
                {
                    cmd.Parameters.AddWithValue("@userName", obj.UserName);
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
    }
}