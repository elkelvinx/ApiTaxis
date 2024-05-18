using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace Servicios
{
    public class ServicioUnit
    {
        public listUnit consultarUnits()
        {
            listUnit list = new listUnit();
            string consulta = "SELECT a.*,m.name as [modelName],b.name as [brandName],d.name+' '+ + d.lm1+ ' '" +
                "+ d.lm2  as adminName " +
                "FROM units as a " +
                "INNER JOIN models as m ON a.model= m.id  " +
                "INNER JOIN brands as b ON m.idBrand=b.id " +
                "INNER JOIN admins as d ON a.admin=d.id;";
            try
            {
                SqlConnection con = ServiciosBD.ObtenerConexion();
                SqlCommand command = new SqlCommand(consulta, con);
                SqlDataReader reader = command.ExecuteReader();
             
                while (reader.Read())
                {
                    Unit obj = new Unit();
                    obj.id = Int16.Parse(reader["id"].ToString());
                    obj.ecoNumber = Int16.Parse(reader["ecoNumber"].ToString());
                    obj.model = Int16.Parse(reader["model"].ToString());
                    obj.yearModel = Int16.Parse(reader["yearModel"].ToString());
                    obj.color = reader["color"].ToString();
                    obj.serie = reader["serie"].ToString();
                    obj.motor = reader["motor"].ToString();
                    obj.plate = reader["plate"].ToString();
                    obj.registerDate = DateTime.Parse(reader["registerDate"].ToString());
                    obj.lastModDate = DateTime.Parse(reader["lastModDate"].ToString());
                    obj.expInsurance = DateTime.Parse(reader["expInsurance"].ToString());
                    obj.admin = Int16.Parse(reader["admin"].ToString());
                    obj.adminName = reader["adminName"].ToString();
                    obj.modelName = reader["modelName"].ToString();
                    obj.brandName = reader["brandName"].ToString();
                    list.Add(obj);
                }
                
            }
            catch (Exception ex)
            {
                // Aquí puedes manejar la excepción, por ejemplo, registrándola en un archivo de log
                Console.WriteLine(ex.ToString());
            }
            return list;
        }
        public Unit consultarUnit(int id)
        {
            string cadena = "SELECT a.*,m.name as [modelName],b.name as  " +
                "[brandName],d.name+' '+ + d.lm1+ ' '+ d.lm2  as adminName FROM units as a " +
                "INNER JOIN models as m ON a.model= m.id " +
                "INNER JOIN brands as b ON m.idBrand=b.id " +
                "INNER JOIN admins as d ON a.admin=d.id " +
                "where a.id=@id;";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand command = new SqlCommand(cadena, con);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();
            Unit obj = new Unit();
            if (reader.Read())
            {
                obj.id = Int16.Parse(reader["id"].ToString());
                obj.ecoNumber = Int16.Parse(reader["ecoNumber"].ToString());
                obj.model = Int16.Parse(reader["model"].ToString());
                obj.yearModel = Int16.Parse(reader["yearModel"].ToString());
                obj.color = reader["color"].ToString();
                obj.serie = reader["serie"].ToString();
                obj.motor = reader["motor"].ToString();
                obj.plate = reader["plate"].ToString();
                obj.registerDate = DateTime.Parse(reader["registerDate"].ToString());
                obj.lastModDate = DateTime.Parse(reader["lastModDate"].ToString());
                obj.expInsurance = DateTime.Parse(reader["expInsurance"].ToString());
                obj.admin = Int16.Parse(reader["admin"].ToString());
                obj.adminName = reader["adminName"].ToString();
                obj.modelName = reader["modelName"].ToString();
                obj.brandName = reader["brandName"].ToString();
            }
            return obj;
        }
        public string insertar(Unit obj)
        {
            string respuesta = "ok";
            string cadena = "INSERT INTO units (ecoNumber,model,yearModel,color,serie,motor,plate,registerDate,lastModDate,expInsurance,admin)VALUES " +
                "(@ecoNumber,@model,@yearModel,@color,@serie,@motor,@plate,@registerDate,@lastModDate,@expInsurance,@idAdmin);";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand(cadena, con);
            cmd.Parameters.AddWithValue("@ecoNumber", obj.ecoNumber);
            cmd.Parameters.AddWithValue("@model", obj.model);
            // cmd.Parameters.AddWithValue("@model", obj.model);
            cmd.Parameters.AddWithValue("@yearModel", obj.yearModel);
            cmd.Parameters.AddWithValue("@color", obj.color);
            cmd.Parameters.AddWithValue("@serie", obj.serie);
            cmd.Parameters.AddWithValue("@motor", obj.motor);
            cmd.Parameters.AddWithValue("@plate", obj.plate);
            cmd.Parameters.AddWithValue("@registerDate", obj.registerDate);
            cmd.Parameters.AddWithValue("@lastModDate", obj.lastModDate);
            cmd.Parameters.AddWithValue("@expInsurance", obj.expInsurance);
            cmd.Parameters.AddWithValue("@idAdmin", obj.admin);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { respuesta = "Error, " + ex.Message.ToString(); }
            return respuesta;
        }
        public string Actualizar(Unit obj)
        {
            DateTime expInsuranceDate;

            string respuesta = "ok";

            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand("update units set ecoNumber=@ecoNumber, model=@model," +
                "yearmodel=@yearModel,color=@color,serie=@serie," +
                "motor=@motor, plate=@plate, lastModDate=@lastModDate," +
                "expInsurance=@expInsurance,admin=@idAdmin " +
                "where id=@id;", con);
            cmd.Parameters.AddWithValue("@ecoNumber", obj.ecoNumber);
            cmd.Parameters.AddWithValue("@model", obj.model);
            cmd.Parameters.AddWithValue("@yearModel", obj.yearModel);
            cmd.Parameters.AddWithValue("@color", obj.color);
            cmd.Parameters.AddWithValue("@serie", obj.serie);
            cmd.Parameters.AddWithValue("@motor", obj.motor);
            cmd.Parameters.AddWithValue("@plate", obj.plate);
            //cmd.Parameters.AddWithValue("@registerDate", obj.registerDate);
            cmd.Parameters.AddWithValue("@lastModDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@expInsurance", obj.expInsurance);
            cmd.Parameters.AddWithValue("@idAdmin", obj.admin);
            cmd.Parameters.AddWithValue("@id", obj.id);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { respuesta = "Error, " + ex.Message.ToString(); }
            return respuesta;
        }
        public string eliminar(int id)
        {
            string respuesta = "ok";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            string cadena = "delete from Units where id=@id";
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
