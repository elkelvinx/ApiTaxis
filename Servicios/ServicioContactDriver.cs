using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Entidades;
using Entidades.Drivers;

namespace Servicios
{
    public class ServicioContactDriver
    {
        public listContactDriver consultarContactDrivers()
        {
            listContactDriver list = new listContactDriver();
            string consulta = "select c.*,r.name as relationS,"+
                "(select s.name from streets as s where s.id = c.st1) as street1," +
                "(select s.name from streets as s where s.id = c.st2) as street2," +
                "(select s.name from streets as s where s.id = c.st3) as street3," +
                "(select s.name from settlements as s where s.id = c.settlement) as settlementS" +
                "from contactsDrivers as c" +
                "INNER JOIN relationship as r" +
                "ON r.id = c.relation";
            try
            {
                SqlConnection con = ServiciosBD.ObtenerConexion();
                SqlCommand command = new SqlCommand(consulta, con);
                SqlDataReader reader = command.ExecuteReader();
             
                while (reader.Read())
                {
                    contactDriver obj = new contactDriver();
                    obj.id = Int16.Parse(reader["id"].ToString());
                    obj.name = reader["name"].ToString();
                    obj.lm1 = reader["lm1"].ToString();
                    obj.lm2 = reader["lm2"].ToString();
                    obj.phone = reader["phone"].ToString();
                    obj.settlement = Int16.Parse(reader["settlement"].ToString());
                    obj.st1 = Int16.Parse(reader["st1"].ToString());
                    obj.st2 = Int16.Parse(reader["st2"].ToString());
                    obj.st3 = Int16.Parse(reader["st3"].ToString());
                    obj.extNumber = Int16.Parse(reader["extNumber"].ToString());
                    obj.relation = Int16.Parse(reader["relation"].ToString());
                    //Direction
                    obj.street1 = reader["street1"].ToString();
                    obj.street2 = reader["street2"].ToString();
                    obj.street3 = reader["street3"].ToString();
                    obj.settlementS = reader["settlementS"].ToString();
                    obj.relationS = reader["relationS"].ToString();
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
        public contactDriver consultarContactDriver(int id)
        {
            string cadena = "select c.*,r.name as relationS," +
                "(select s.name from streets as s where s.id = c.st1) as street1," +
                "(select s.name from streets as s where s.id = c.st2) as street2," +
                "(select s.name from streets as s where s.id = c.st3) as street3," +
                "(select s.name from settlements as s where s.id = c.settlement) as settlementS" +
                " from contactsDrivers as c " +
                "INNER JOIN relationship as r  ON r.id= c.relation where c.id= @id";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand command = new SqlCommand(cadena, con);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();
            contactDriver obj = new contactDriver();
            if (reader.Read())
            {
                obj.id = Int16.Parse(reader["id"].ToString());
                obj.name = reader["name"].ToString();
                obj.lm1 = reader["lm1"].ToString();
                obj.lm2 = reader["lm2"].ToString();
                obj.phone = reader["phone"].ToString();
                obj.st1 = Int16.Parse(reader["st1"].ToString());
                obj.st2 = Int16.Parse(reader["st2"].ToString());
                obj.st3 = Int16.Parse(reader["st3"].ToString());
                obj.settlement = Int16.Parse(reader["settlement"].ToString());
                obj.extNumber = Int16.Parse(reader["extNumber"].ToString());
                obj.relation = Int16.Parse(reader["relation"].ToString());
                obj.street1 = reader["street1"].ToString();
                obj.street2 = reader["street2"].ToString();
                obj.street3 = reader["street3"].ToString();
                obj.settlementS = reader["settlementS"].ToString();
                obj.relationS = reader["relationS"].ToString();
            }
            return obj;
        }
        public string insertar(contactDriver obj)
        {
            string respuesta = "ok";
            string cadena = "INSERT INTO contactsDrivers " +
                "(name, lm1, lm2, phone, st1, st2, st3, " +
                "settlement, extNumber,relation) " +
                "VALUES (@name,@lm1,@lm2,@phone,@st1,@st2,@st3," +
                "@settlement,@extNumber,@relation);";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand(cadena, con);
            cmd.Parameters.AddWithValue("@id", obj.id);
            cmd.Parameters.AddWithValue("@name", obj.name);
            cmd.Parameters.AddWithValue("@lm1", obj.lm1);
            cmd.Parameters.AddWithValue("@lm2", obj.lm2);
            cmd.Parameters.AddWithValue("@phone", obj.phone);
            cmd.Parameters.AddWithValue("@st1", obj.st1);
            cmd.Parameters.AddWithValue("@st2", obj.st2);
            cmd.Parameters.AddWithValue("@st3", obj.st3);
            cmd.Parameters.AddWithValue("@settlement", obj.settlement);
            cmd.Parameters.AddWithValue("@extNumber", obj.extNumber);
            cmd.Parameters.AddWithValue("@relation", obj.relation);            
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { respuesta = "Error, " + ex.Message.ToString(); }
            return respuesta;
        }
        public string Actualizar(contactDriver obj)
        {

            string respuesta = "ok";

            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand(
                "update contactsDrivers set "+
                "name = @name," +
                "lm1 = @lm1," +
                "lm2 = @lm2," +
                "phone = @phone," +
                "st1 = @st1, " +
                "st2 = @st2, " +
                "st3 = @st3, " +
                "settlement = @settlement, " +
                "extNumber = @extNumber, " +
                "relation = @relation " +
                "where id = @id; ", con);
            cmd.Parameters.AddWithValue("@id", obj.id);
            cmd.Parameters.AddWithValue("@name", obj.name);
            cmd.Parameters.AddWithValue("@lm1", obj.lm1);
            cmd.Parameters.AddWithValue("@lm2", obj.lm2);
            cmd.Parameters.AddWithValue("@phone", obj.phone);
            cmd.Parameters.AddWithValue("@st1", obj.st1);
            cmd.Parameters.AddWithValue("@st2", obj.st2);
            cmd.Parameters.AddWithValue("@st3", obj.st3);
            cmd.Parameters.AddWithValue("@settlement", obj.settlement);
            cmd.Parameters.AddWithValue("@extNumber", obj.extNumber);
            cmd.Parameters.AddWithValue("@relation", obj.relation);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { respuesta = "Error, " + ex.Message.ToString(); }
            return respuesta;
        }
        public string eliminar(int id)
        {
            string respuesta = "ok";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            string cadena = "delete from contactsDrivers where id=@id";
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
