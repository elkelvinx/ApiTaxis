using Entidades;
using Entidades.Arrays;
using Entidades.Cars;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioArrayCR
    {
        ///////////////////  READS   /////////////////////////
        public listArray consultarNombreAdmin()
        {
            string cadena = "select id,name+' '+lm1+' '+lm2 as name from admins";
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
        public listArray consultarNombreDriver()
        {
            string cadena = "select id,name+' '+lm1+' '+lm2 as name from drivers";
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
        public listArray consultarNombreInsurance()
        {
            string cadena = "select id,name from insurers";
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
        public listArray consultarTypeInsurance()
        {
            string cadena = "select id,name from sinistersTypes";
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

        public listArray consultarStatus()
        {
            string cadena = "select * from status";
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
        public listArray consultarBrandCar()
        {
            string cadena = "select * from brands";
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
        public listModelsCar consultarModelCar()
        {
            string cadena = "select m.id,m.name,m.idbrand,b.name as brand" +
                " from models as m INNER JOIN brands as b on b.id=m.idBrand order by b.name";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand command = new SqlCommand(cadena, con);
            SqlDataReader reader = command.ExecuteReader();
            listModelsCar list = new listModelsCar();
            while (reader.Read())
            {
                ModelsCar obj = new ModelsCar();
                obj.id = Int16.Parse(reader["id"].ToString());
                obj.name = reader["name"].ToString();
                obj.brandName= reader["brand"].ToString();
                obj.idBrand = Int16.Parse(reader["idBrand"].ToString());
                list.Add(obj);
            }
            return list;
        }
        ///////////////////  CREATE   /////////////////////////
        public string insertarStatus(Arrays obj)
        {
            string respuesta = "ok";
            string cadena = "insert into status (name)" +
                " Values (@name)";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand(cadena, con);
            cmd.Parameters.AddWithValue("@name", obj.name);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { respuesta = "Error, " + ex.Message.ToString(); }
            return respuesta;
        }
        public string insertarSinisterType(Arrays obj)
        {
            string respuesta = "ok";
            string cadena = "insert into sinistersType (name)" +
                " Values (@name)";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand(cadena, con);
            cmd.Parameters.AddWithValue("@name", obj.name);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { respuesta = "Error, " + ex.Message.ToString(); }
            return respuesta;
        }
        public string insertarInsuranceName(Arrays obj)
        {
            string respuesta = "ok";
            string cadena = "insert into insurers (name)" +
                " Values (@name)";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand(cadena, con);
            cmd.Parameters.AddWithValue("@name", obj.name);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { respuesta = "Error, " + ex.Message.ToString(); }
            return respuesta;
        }
    } 
}
