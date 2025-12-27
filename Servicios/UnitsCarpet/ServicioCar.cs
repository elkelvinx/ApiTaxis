using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using Entidades.Cars;

namespace Servicios
{
    public class ServicioCar
    {
        public listBrand consultarBrands()
        {
            string consulta = "SELECT * FROM brands;";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand command = new SqlCommand(consulta, con);
            SqlDataReader reader = command.ExecuteReader();
            listBrand list = new listBrand();
            while (reader.Read())
            {
                Brand obj = new Brand();
                obj.id = Int16.Parse(reader["id"].ToString());
                obj.name = reader["name"].ToString();
                list.Add(obj);
            }
            return list;
        }
        public string insertar(Brand obj)
        {
            string respuesta = "ok";
            string cadena = "insert into brands (name)" +
                " Values (@name)";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand(cadena, con);
            cmd.Parameters.AddWithValue("@name", obj.name);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { respuesta = "Error, " + ex.Message.ToString(); }
            return respuesta;
        }
        public string Actualizar(Brand obj)
        {
            string respuesta = "ok";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand("update brands set name=@name where id=@id", con);
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
            string cadena = "delete from brands where id=@id";
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
        /////////////MODELS/////////////
        public listModelsCar consultarModels()
        {
            string consulta = "SELECT a.*,b.name as BrandName " +
                "FROM models as a Inner Join brands as b ON a.idBrand=b.id " +
                "order by BrandName ASC;";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand command = new SqlCommand(consulta, con);
            SqlDataReader reader = command.ExecuteReader();
            listModelsCar list = new listModelsCar();
            while (reader.Read())
            {
                ModelsCar obj = new ModelsCar();
                obj.id = Int16.Parse(reader["id"].ToString());
                obj.idBrand = Int16.Parse(reader["idBrand"].ToString());
                obj.name = reader["name"].ToString();
                obj.brandName = reader["brandName"].ToString();
                list.Add(obj);
            }
            return list;
        }
        public string insertar(ModelsCar obj)
        {
            string respuesta = "ok";
            string cadena = "insert into models (name,idBrand)" +
                " Values (@name,@idBrand)";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand(cadena, con);
            cmd.Parameters.AddWithValue("@name", obj.name);
            cmd.Parameters.AddWithValue("@idBrand", obj.idBrand);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { respuesta = "Error, " + ex.Message.ToString(); }
            return respuesta;
        }
        public string Actualizar(ModelsCar obj)
        {
            string respuesta = "ok";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand("update models set name=@name,idBrand=@idBrand where id=@id", con);
            cmd.Parameters.AddWithValue("@id", obj.id);
            cmd.Parameters.AddWithValue("@name", obj.name);
            cmd.Parameters.AddWithValue("@idBrand", obj.idBrand);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { respuesta = "Error, " + ex.Message.ToString(); }
            return respuesta;
        }
        public string eliminarModel(int id)
        {
            string respuesta = "ok";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            string cadena = "delete from models where id=@id";
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
