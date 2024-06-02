using Entidades;
using Entidades.LogIn;
using ServicioEncriptacion;
using EO.WebBrowser.DOM;
using Servicios.LogIn;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Entidades.Response;

namespace Servicios
{
    public class ServicioLogIn : IUserService
    {
        public AppDomainInitializer AppDomainInitializer { get; set; }
        public ServicioLogIn() { }
        
        public void LogIn(Hash codigo)
        {
        
        }

        public static string ConvertirSha256(string texto) {
            //using System. Text
            //USAR LA REFERENCIA DE "Systen. Security Cryptography"
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));
                foreach (byte b in result)
                    Sb.Append(b.ToString("x2")); 
            }
            return Sb.ToString();
        }

          public listDrivers consultarUsers()
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
        public string Actualizar(Driver obj)
        {

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
        //corrige esto
        public User Auth(Encriptacion encriptacion)
        {
            string password = encriptacion.GetSHA256("d");
            throw new NotImplementedException();
        }

        public Respuesta Auth(AuthRequest model)
        {
            throw new NotImplementedException();
        }
    }

}

