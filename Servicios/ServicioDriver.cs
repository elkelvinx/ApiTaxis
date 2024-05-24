using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioDriver
    {
        
          public listDrivers consultarDrivers()
         {
             string cadena = "select a.*," +
                 "(select s.name from streets as s where s.id = a.st1) as street1," +
                 "(select s.name from streets as s where s.id = a.st2) as street2," +
                 "(select s.name from streets as s where s.id = a.st3) as street3," +
                 "(select s.name from settlements as s where s.id = a.settlement) as settlementS," +
                 "(select s.name from admins as s where s.id = a.admin) as adminName, " +
                 "(select s.name from status as s where s.id = a.status)as statusS " +
                 "from drivers as a;";
            /*SqlConnection con = ServiciosBD.ObtenerConexion("hola","hola");*/
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand command = new SqlCommand(cadena, con);
             SqlDataReader reader = command.ExecuteReader();
             listDrivers list = new listDrivers();
             while (reader.Read())
             {
                 Driver obj = new Driver();
                 obj.id = Int16.Parse(reader["id"].ToString());
                 obj.name = reader["name"].ToString();
                 obj.lm1 = reader["lm1"].ToString();
                 obj.lm2 = reader["lm2"].ToString();
                 obj.birth = DateTime.Parse(reader["birth"].ToString());
                 obj.hireDate = DateTime.Parse(reader["hireDate"].ToString());
                 obj.lastModD = DateTime.Parse(reader["lastModD"].ToString());
                 obj.password = reader["password"].ToString();
                 obj.phone = reader["phone"].ToString();
                 obj.settlement = Int16.Parse(reader["settlement"].ToString());
                 obj.st1 = Int16.Parse(reader["st1"].ToString());
                 obj.st2 = Int16.Parse(reader["st2"].ToString());
                 obj.st3 = Int16.Parse(reader["st3"].ToString());
                 obj.extNumber = Int16.Parse(reader["extNumber"].ToString());
                 obj.admin = Int16.Parse(reader["admin"].ToString());
                 obj.licenseEx = DateTime.Parse(reader["licenseEx"].ToString());
                 obj.ingressPay = Int16.Parse(reader["ingressPay"].ToString());
                 obj.status = Int16.Parse(reader["status"].ToString());
                 //Direction
                 obj.street1 = reader["street1"].ToString();
                 obj.street2 = reader["street2"].ToString();
                 obj.street3 = reader["street3"].ToString();
                 obj.settlementS = reader["settlementS"].ToString();
                 obj.adminName = reader["adminName"].ToString();
                obj.statusS = reader["statusS"].ToString();
                list.Add(obj);
             }
             con.Close();
            
             return list;
         }
        
        /*   public listDrivers ConsultarDrivers()
           {
               string cadena = "SELECT a.*," +
                   "(SELECT s.name FROM streets AS s WHERE s.id = a.st1) AS street1," +
                   "(SELECT s.name FROM streets AS s WHERE s.id = a.st2) AS street2," +
                   "(SELECT s.name FROM streets AS s WHERE s.id = a.st3) AS street3," +
                   "(SELECT s.name FROM settlements AS s WHERE s.id = a.settlement) AS settlementS," +
                   "(SELECT s.name FROM admins AS s WHERE s.id = a.admin) AS adminName " +
                   "FROM drivers AS a;";

               using (SqlConnection con = ServiciosBD.ObtenerConexion())
               {
                   con.Open();

               using (SqlCommand command = new SqlCommand(cadena, con))
               {

                   using (SqlDataReader reader = command.ExecuteReader())
                   {
                       listDrivers list = new listDrivers();
                       while (reader.Read())
                       {
                           Driver obj = new Driver();
                           obj.id = Int16.Parse(reader["id"].ToString());
                           obj.name = reader["name"].ToString();
                           obj.lm1 = reader["lm1"].ToString();
                           obj.lm2 = reader["lm2"].ToString();
                           obj.birth = DateTime.Parse(reader["birth"].ToString());
                           obj.hireDate = DateTime.Parse(reader["hireDate"].ToString());
                           obj.lastModD = DateTime.Parse(reader["lastModD"].ToString());
                           obj.password = reader["password"].ToString();
                           obj.phone = reader["phone"].ToString();
                           obj.settlement = Int16.Parse(reader["settlement"].ToString());
                           obj.st1 = Int16.Parse(reader["st1"].ToString());
                           obj.st2 = Int16.Parse(reader["st2"].ToString());
                           obj.st3 = Int16.Parse(reader["st3"].ToString());
                           obj.extNumber = Int16.Parse(reader["extNumber"].ToString());
                           obj.admin = Int16.Parse(reader["admin"].ToString());
                           obj.licenseEx = DateTime.Parse(reader["licenseEx"].ToString());
                           obj.ingressPay = Int16.Parse(reader["ingressPay"].ToString());
                           obj.status = Int16.Parse(reader["status"].ToString());
                           //Direction
                           obj.street1 = reader["street1"].ToString();
                           obj.street2 = reader["street2"].ToString();
                           obj.street3 = reader["street3"].ToString();
                           obj.settlementS = reader["settlementS"].ToString();
                           obj.adminName = reader["adminName"].ToString();
                           list.Add(obj);
                       }
                       return list;
                   }
               }
               }
           }*/

        public Driver consultarDriver(int id)
        {
            string cadena = "select a.*," +
                "(select s.name from streets as s where s.id = a.st1) as street1," +
                "(select s.name from streets as s where s.id = a.st2) as street2," +
                "(select s.name from streets as s where s.id = a.st3) as street3," +
                "(select s.name from settlements as s where s.id = a.settlement) as settlementS," +
                "(select s.name+' '+s.lm1+' '+s.lm2 from admins as s where s.id = a.admin) as adminName, " +
                "(select s.name from status as s where s.id = a.status) as statusS " +
                "from drivers as a where a.id = @id;";           
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand command = new SqlCommand(cadena, con);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();
            Driver obj = new Driver();

            if (reader.Read())
            {
                obj.id = Int16.Parse(reader["id"].ToString());
                obj.name = reader["name"].ToString();
                obj.lm1 = reader["lm1"].ToString();
                obj.lm2 = reader["lm2"].ToString();
                obj.birth = DateTime.Parse(reader["birth"].ToString());
                obj.hireDate = DateTime.Parse(reader["hireDate"].ToString());
                obj.lastModD = DateTime.Parse(reader["lastModD"].ToString());
                obj.password = reader["password"].ToString();
                obj.phone = reader["phone"].ToString();
                obj.settlement = Int16.Parse(reader["settlement"].ToString());
                obj.st1 = Int16.Parse(reader["st1"].ToString());
                obj.st2 = Int16.Parse(reader["st2"].ToString());
                obj.st3 = Int16.Parse(reader["st3"].ToString());              
                obj.extNumber = Int16.Parse(reader["extNumber"].ToString());
                obj.admin = Int16.Parse(reader["admin"].ToString());
                obj.licenseEx = DateTime.Parse(reader["licenseEx"].ToString());
                obj.ingressPay = Int16.Parse(reader["ingressPay"].ToString());
                obj.status = Int16.Parse(reader["status"].ToString());
                    try{obj.contactDrivers = Int16.Parse(reader["contactDrivers"].ToString());}
                    catch {obj.contactDrivers= 0;}
                //Direction
                obj.street1 = reader["street1"].ToString();
                obj.street2 = reader["street2"].ToString();
                obj.street3 = reader["street3"].ToString();
                obj.settlementS = reader["settlementS"].ToString();
                obj.adminName = reader["adminName"].ToString();
                obj.statusS = reader["statusS"].ToString();
            }
            con.Close();
            return obj;
        }

        public string insertar(Driver obj)
        {
            string respuesta = "ok";
            string cadena = "insert into drivers " +
                "(name,lm1,lm2,phone,st1,st2,st3,settlement,extNumber,birth,hireDate,lastModD,Password,admin,licenseEx,ingressPay,status)" +
                "Values(@name,@lm1,@lm2,@phone,@st1,@st2,@st3,@settlement,@extNumber,@birth,@hireDate,@lastModD,@password,@admin,@licenseEx,@ingressPay,@status) ";
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
            cmd.Parameters.AddWithValue("@birth", obj.birth);
            cmd.Parameters.AddWithValue("@hireDate", DateTime.Now.Date);
            cmd.Parameters.AddWithValue("@lastModD", DateTime.Now.Date);
            cmd.Parameters.AddWithValue("@password", obj.password);
            cmd.Parameters.AddWithValue("@admin", obj.admin);
            cmd.Parameters.AddWithValue("@licenseEx", obj.licenseEx);
            cmd.Parameters.AddWithValue("@ingressPay", obj.ingressPay);
            cmd.Parameters.AddWithValue("@status", obj.status);
            cmd.ExecuteNonQuery();
            /*
               try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { respuesta = "Error, " + ex.Message.ToString(); }*/
                con.Close();

            return respuesta;
        }
        ///FALLAA CHECA ACTUALIZAR
        public string Actualizar(Driver obj)
        {
            //el registerD estoy dudando si se podria modificar
            //por que es algo que solo se modifica una vez
            string respuesta = "ok";

            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand("update drivers set name=@name,lm1=@lm1,lm2=@lm2," +
                "phone=@phone,st1=@st1,st2=@st2,st3=@st3,settlement=@settlement,extNumber=@extNumber,birth=@birth,hireDate=@hireDate," +
                "lastModD=@lastModD,password=@password,admin=@admin,licenseEx=@licenseEx,ingressPay=@ingressPay,status=@status where id=@id", con);
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
            cmd.Parameters.AddWithValue("@hireDate", obj.hireDate);
            cmd.Parameters.AddWithValue("@lastModD", obj.lastModD);
            cmd.Parameters.AddWithValue("@password", obj.password);
            cmd.Parameters.AddWithValue("@admin", obj.admin);
            cmd.Parameters.AddWithValue("@licenseEx", obj.licenseEx);
            cmd.Parameters.AddWithValue("@ingressPay", obj.ingressPay);
            cmd.Parameters.AddWithValue("@status", obj.status);
            cmd.ExecuteNonQuery();
            con.Close();
            return respuesta;
        }
        public string eliminar(int id)
        {
            string respuesta = "ok";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            string cadena = "delete from drivers where id=@id";
            SqlCommand cmd = new SqlCommand(cadena, con);
            cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();            
            con.Close();
            return respuesta;
        }
    }

}

