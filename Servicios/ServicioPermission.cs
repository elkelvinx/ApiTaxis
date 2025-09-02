using Entidades;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioPermission
    {
        public Permission consultarPermissionaire(int id)
        {
            string cadena = "select a.*, " +
                " (select s.name from streets as s where s.id = a.st1) as street1," +
                " (select s.name from streets as s where s.id = a.st2) as street2," +
                " (select s.name from streets as s where s.id = a.st3) as street3, " +
                " (select s.name from settlements as s where s.id = a.settlement) as settlementS" +
                " from permissionaires as a inner join streets as s" +
                " on a.st1 = s.id where a.id = @id;";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand command = new SqlCommand(cadena, con);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();
            Permission obj = new Permission();
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
                obj.registerD = DateTime.Parse(reader["registerD"].ToString());
                obj.lastModDate = DateTime.Parse(reader["lastModDate"].ToString());
                obj.birth = DateTime.Parse(reader["birth"].ToString());
                //nombres calles
                obj.street1 = reader["street1"].ToString();
                obj.street2 = reader["street2"].ToString();
                obj.street3 = reader["street3"].ToString();
                obj.settlementS = reader["settlementS"].ToString();
            }
            return obj;
        }
        public listPermission consultarPermissions()
        {
            string cadena = "	select a.*," +
                " (select s.name from streets as s where s.id = a.st1) as street1, " +
                " (select s.name from streets as s where s.id = a.st2) as street2,  " +
                " (select s.name from streets as s where s.id = a.st3) as street3,   " +
                " (select s.name from settlements as s where s.id = a.settlement) as settlementS " +
                " from permissionaires as a";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand command = new SqlCommand(cadena, con);
            SqlDataReader reader = command.ExecuteReader();
            listPermission admins = new listPermission();
            while (reader.Read())
            {
                Permission obj = new Permission();

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
                obj.registerD = DateTime.Parse(reader["lastModDate"].ToString());
                obj.lastModDate = DateTime.Parse(reader["lastModDate"].ToString());
                obj.birth = DateTime.Parse(reader["birth"].ToString());
                //nombres calles
                obj.street1 = reader["street1"].ToString();
                obj.street2 = reader["street2"].ToString();
                obj.street3 = reader["street3"].ToString();
                obj.settlementS = reader["settlementS"].ToString();
                admins.Add(obj);

            }
            return admins;
        }


        public string insertar(Permission obj)
        {
            string respuesta = "ok";
            string cadena = "insert into permissionaires (name,lm1,lm2,phone,st1,st2,st3,settlement,extNumber,registerD,lastModDate,birth)" +
                " Values (@name,@lm1,@lm2,@phone,@st1,@st2,@st3,@settlement,@extNumber,@registerD,@lastModDate,@birth)";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand(cadena, con);
           // cmd.Parameters.AddWithValue("@id", obj.id);
            cmd.Parameters.AddWithValue("@name", obj.name);
            cmd.Parameters.AddWithValue("@lm1", obj.lm1);
            cmd.Parameters.AddWithValue("@lm2", obj.lm2);
            cmd.Parameters.AddWithValue("@phone", obj.phone);
            cmd.Parameters.AddWithValue("@st1", obj.st1);
            cmd.Parameters.AddWithValue("@st2", obj.st2);
            cmd.Parameters.AddWithValue("@st3", obj.st3);
            cmd.Parameters.AddWithValue("@settlement", obj.settlement);
            cmd.Parameters.AddWithValue("@extNumber", obj.extNumber);
            cmd.Parameters.AddWithValue("@birth", obj.birth);
            cmd.Parameters.AddWithValue("@registerD", DateTime.Now.Date);
            cmd.Parameters.AddWithValue("@lastModDate", DateTime.Now.Date);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { respuesta = "Error, " + ex.Message.ToString(); }
            return respuesta;
        }

        public string Actualizar(Permission obj)
        {
            //el registerD estoy dudando si se podria modificar
            //por que es algo que solo se modifica una vez
            string respuesta = "ok";

            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand("update permissionaires set " +
                "name=@name, lm1=@lm1, lm2=@lm2 ,phone=@phone, st1=@st1," +
                "st2=@st2,st3=@st3,settlement=@settlement,extNumber=@extNumber," +
                "lastModDate=@lastModDate,birth=@birth" +
                " where id=@id", con);
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
            cmd.Parameters.AddWithValue("@birth", obj.birth);
            //dudo de registerD
            cmd.Parameters.AddWithValue("@lastModDate", DateTime.Now.Date);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { respuesta = "Error, " + ex.Message.ToString(); }
            return respuesta;
        }
        public string eliminar(int id)
        {
            string respuesta = "ok";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            string cadena = "delete from permissionaires where id=@id";
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

