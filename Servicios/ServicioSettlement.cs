using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servicios.Logs;
using Entidades;

namespace Servicios
{
    public class ServicioSettlement
    {
        /*
           private readonly ServicioChangeLog servicioChangeLog;
          public ServicioSettlement() { 
          servicioChangeLog = new ServicioChangeLog();
          }
         */
        public listSettle consultarColonias()
        {
            string consulta = "select * from settlements order by name ASC";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand command = new SqlCommand(consulta, con);
            SqlDataReader reader = command.ExecuteReader();
            listSettle list = new listSettle();
            while (reader.Read())
            {
                settlement obj = new settlement();
                obj.id = Int16.Parse(reader["id"].ToString());
                obj.name = reader["name"].ToString();
                list.Add(obj);
            }
            SqlConnection.ClearPool(con);
            con.Close();
            return list;
        }
        public listSettle consultarNombre()
        {
            string cadena = "select id,name from settlements";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand command = new SqlCommand(cadena, con);
            SqlDataReader reader = command.ExecuteReader();
            listSettle list = new listSettle();
            while (reader.Read())
            {
                settlement obj = new settlement();
                obj.id = Int16.Parse(reader["id"].ToString());
                obj.name = reader["name"].ToString();
                list.Add(obj);
            }
            SqlConnection.ClearPool(con);
            con.Close();
            return list;
        }
        public string insertar(settlement obj)
        {          
            string respuesta = "ok";
            string cadena = "insert into settlements (name)" +
                " Values (@name)";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand(cadena, con);
            cmd.Parameters.AddWithValue("@name", obj.name);            
             
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) {
                SqlConnection.ClearPool(con);
                con.Close();
                ServicioErrorLogs.RegisterErrorLog("Settlement",1,cmd,ex.Message);
                respuesta = "Error, " + ex.Message.ToString(); 
                return respuesta;
            }
            ServicioChangeLog.UpdateTriggerChangeLog("Settlement", 1, cmd);
            SqlConnection.ClearPool(con);
            con.Close();
            return respuesta;
        }
        public string Actualizar(settlement obj)
        {
            string respuesta = "ok";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand("update settlements set name=@name where id=@id", con);
            cmd.Parameters.AddWithValue("@id", obj.id);
            cmd.Parameters.AddWithValue("@name", obj.name);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { respuesta = "Error, " + ex.Message.ToString(); }
            return respuesta;
        }
        public string eliminar(int id)
        {
            string respuesta = "ok";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            string cadena = "delete from settlements where id=@id";
            SqlCommand cmd = new SqlCommand(cadena, con);
            cmd.Parameters.AddWithValue("@id", id);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                SqlConnection.ClearPool(con);
                con.Close();
                respuesta = "Error " + ex.Message.ToString();
            }
            SqlConnection.ClearPool(con);
            con.Close();
            return respuesta;
        }
    }
}
