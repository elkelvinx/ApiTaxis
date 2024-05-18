using Entidades.ModelsCar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.Arrays;
using Entidades;

namespace Servicios.Relation
{
    public class ServicioRelation
    {
        public listArray consultaRelations()
        {
            string cadena = "select * from relationship";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand command = new SqlCommand(cadena, con);
            SqlDataReader reader = command.ExecuteReader();
            listArray list = new listArray();
            while (reader.Read())
            {
                Arrays obj = new Arrays();
                obj.id = Int16.Parse(reader["id"].ToString());
                obj.name = reader["name"].ToString();
                list.Add(obj);
            }
            return list;
        }
        public string insertar(Arrays obj)
        {
            string respuesta = "ok";
            string cadena = "insert into relationship (name)" +
                " Values (@name)";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand(cadena, con);
            cmd.Parameters.AddWithValue("@name", obj.name);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { respuesta = "Error, " + ex.Message.ToString(); }
            return respuesta;
        }
        public string actualizar(Arrays obj)
        {
            string respuesta = "ok";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand("update relationship set name=@name where id=@id", con);
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
            string cadena = "delete from relationship where id=@id";
            SqlCommand cmd = new SqlCommand(cadena, con);
            cmd.Parameters.AddWithValue("@id", id);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                respuesta = "Error " + ex.Message.ToString();
            }
            return respuesta;
        }
    }
}
