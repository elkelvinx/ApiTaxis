using System;
using System.Collections.Generic;
using System.Data;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;
using SqlDataReader = Microsoft.Data.SqlClient.SqlDataReader;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using Entidades.Cars;

namespace Servicios
{
    public class ServicioSinister
    {
        public listSinister consultarSiniestros()
        {
            listSinister list = new listSinister();
            string consulta = "select a.id,a.idUnit,typeSinister,a.st1,a.st2,a.st3," +
                "a.settlement,a.insurance,a.OBS, a.dateEvent,a.typeInsurance," +
                "a.workShop,a.admins,a.driver,a.crashCounterPart," +
                " CASE a.worl WHEN 0 THEN 'perdido' WHEN 1 THEN 'ganado' ELSE 'indefinido' END as worl," +
                " CASE a.personInvolved WHEN 0 THEN 'Driver'  WHEN 1 THEN 'Administrador' ELSE 'otro' END as personInvolved," +
                " CASE a.insuranceAplication WHEN 0 THEN 'Aplica'  WHEN 1 THEN 'No Aplica' ELSE 'otro' END as insuranceAplication," +
                "(select s.ecoNumber from units as s where s.id = a.idUnit) as ecoNumber," +
                "(select s.name from sinistersTypes as s where s.id = a.typeSinister) as typeSinisterS," +
                "(select s.name from insurers as s where s.id = a.insurance) as insuranceS," +
                "(select s.name+' '+s.lm1+' '+s.lm2 from admins as s where s.id = a.admins) as adminName," +
                "(select s.name+' '+s.lm1+' '+s.lm2 from admins as s where s.id = a.driver) as driverName," +
                "(select s.name from streets as s where s.id = a.st1) as street1," +
                "(select s.name from streets as s where s.id = a.st2) as street2," +
                "(select s.name from streets as s where s.id = a.st3) as street3," +
                "(select s.name from settlements as s where s.id = a.settlement) as settlementS " +
                "from sinisters as a;";
            try
            {
                SqlConnection con = ServiciosBD.ObtenerConexion();
                SqlCommand command = new SqlCommand(consulta, con);
                SqlDataReader reader = command.ExecuteReader();
             
                while (reader.Read())
                {
                    Sinister obj = new Sinister();
                    obj.id = Int16.Parse(reader["id"].ToString());
                    obj.idUnit = Int16.Parse(reader["idUnit"].ToString());
                    obj.ecoNumber = Int16.Parse(reader["ecoNumber"].ToString());
                    obj.insurance = Int16.Parse(reader["insurance"].ToString());
                    obj.settlement = Int16.Parse(reader["settlement"].ToString());
                    obj.st1 = Int16.Parse(reader["st1"].ToString());
                    obj.st2 = Int16.Parse(reader["st2"].ToString());
                    obj.st3 = Int16.Parse(reader["st3"].ToString());
                    obj.observations = reader["OBS"].ToString();
                    obj.admin = Int16.Parse(reader["admins"].ToString());
                    obj.driver = Int16.Parse(reader["driver"].ToString());
                    obj.typeSinister = Int16.Parse(reader["typeSinister"].ToString());
                    obj.dateEvent = DateTime.Parse(reader["dateEvent"].ToString());
                    obj.typeInsurance = reader["typeInsurance"].ToString();
                    obj.workShop = reader["workShop"].ToString();
                    obj.typeSinisterS = reader["typeSinisterS"].ToString();
                    obj.insuranceS = reader["insuranceS"].ToString();
                    obj.crashCounterPart = reader["crashCounterPart"].ToString();
                    obj.winOrLooseS = reader["worl"].ToString();
                    obj.personInvolvedS = reader["personInvolved"].ToString();
                    obj.insuranceAplicationS = reader["insuranceAplication"].ToString();
                    //Direction
                    obj.adminS = reader["adminName"].ToString();
                    obj.driverS = reader["driverName"].ToString();
                    obj.street1 = reader["street1"].ToString();
                    obj.street2 = reader["street2"].ToString();
                    obj.street3 = reader["street3"].ToString();
                    obj.settlementS = reader["settlementS"].ToString();
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
        public Sinister consultarSiniestro(int id)
        {
            string consulta = "select a.id,a.idUnit,typeSinister,a.st1,a.st2,a.st3," +
                "a.settlement,a.insurance,a.OBS, a.dateEvent,a.typeInsurance," +
                "a.workShop,a.admins,a.driver,a.crashCounterPart," +
                " CASE a.worl WHEN 0 THEN 'perdido' WHEN 1 THEN 'ganado' ELSE 'indefinido' END as worl," +
                " CASE a.personInvolved WHEN 0 THEN 'Driver'  WHEN 1 THEN 'Administrador' ELSE 'otro' END as personInvolved," +
                " CASE a.insuranceAplication WHEN 0 THEN 'Aplica'  WHEN 1 THEN 'No Aplica' ELSE 'otro' END as insuranceAplication," +
                "(select s.ecoNumber from units as s where s.id = a.idUnit) as ecoNumber," +
                "(select s.name from sinistersTypes as s where s.id = a.typeSinister) as typeSinisterS," +
                "(select s.name from insurers as s where s.id = a.insurance) as insuranceS," +
                "(select s.name+' '+s.lm1+' '+s.lm2 from admins as s where s.id = a.admins) as adminName," +
                "(select s.name+' '+s.lm1+' '+s.lm2 from admins as s where s.id = a.driver) as driverName," +
                "(select s.name from streets as s where s.id = a.st1) as street1," +
                "(select s.name from streets as s where s.id = a.st2) as street2," +
                "(select s.name from streets as s where s.id = a.st3) as street3," +
                "(select s.name from settlements as s where s.id = a.settlement) as settlementS " +
                "from sinisters as a where a.id = @id;";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand command = new SqlCommand(consulta, con);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();
            Sinister obj = new Sinister();
            if (reader.Read())
            {
                obj.id = Int16.Parse(reader["id"].ToString());
                obj.idUnit = Int16.Parse(reader["idUnit"].ToString());
                obj.ecoNumber = Int16.Parse(reader["ecoNumber"].ToString());
                obj.insurance = Int16.Parse(reader["insurance"].ToString());
                obj.settlement = Int16.Parse(reader["settlement"].ToString());
                obj.st1 = Int16.Parse(reader["st1"].ToString());
                obj.st2 = Int16.Parse(reader["st2"].ToString());
                obj.st3 = Int16.Parse(reader["st3"].ToString());
                obj.observations = reader["OBS"].ToString();
                obj.admin = Int16.Parse(reader["admins"].ToString());
                obj.driver= Int16.Parse(reader["driver"].ToString());
                obj.typeSinister = Int16.Parse(reader["typeSinister"].ToString());
                obj.dateEvent = DateTime.Parse(reader["dateEvent"].ToString());
                obj.typeInsurance = reader["typeInsurance"].ToString();
                obj.workShop = reader["workShop"].ToString();
                obj.typeSinisterS = reader["typeSinisterS"].ToString();
                obj.insuranceS = reader["insuranceS"].ToString();
                obj.crashCounterPart = reader["crashCounterPart"].ToString();
                obj.winOrLooseS = reader["worl"].ToString();
                obj.personInvolvedS = reader["personInvolved"].ToString();
                obj.insuranceAplicationS = reader["insuranceAplication"].ToString();
                //Direction
                obj.adminS= reader["adminName"].ToString();
                obj.driverS= reader["driverName"].ToString();
                obj.street1 = reader["street1"].ToString();
                obj.street2 = reader["street2"].ToString();
                obj.street3 = reader["street3"].ToString();
                obj.settlementS = reader["settlementS"].ToString();
            }
            return obj;
        }
        public string insertar(Sinister obj)
        {
            string respuesta = "ok";
            string cadena = "insert into sinisters" +
                "(idUnit,typeSinister,st1,st2,st3,settlement,worl,insurance,dateEvent," +
                "typeInsurance,workShop,admins,driver,personInvolved,insuranceAplication,crashCounterPart,OBS)" +
                "VALUES (@idUnit,@typeSinister,@st1,@st2,@st3,@settlement,@worl,@insurance,@dateEvent," +
                "@typeInsurance,@workShop,@admins,@driver,@personInvolved,@insuranceAplication,@crashCounterPart,@OBS);";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand(cadena, con);
            cmd.Parameters.AddWithValue("@idUnit", obj.idUnit);
            cmd.Parameters.AddWithValue("@typeSinister", obj.typeSinister);
            cmd.Parameters.AddWithValue("@st1", obj.st1);
            cmd.Parameters.AddWithValue("@st2", obj.st2);
            cmd.Parameters.AddWithValue("@st3", obj.st3);
            cmd.Parameters.AddWithValue("@settlement", obj.settlement);
            cmd.Parameters.AddWithValue("@worl", obj.winOrLoose);
            cmd.Parameters.AddWithValue("@insurance", obj.insurance);
            cmd.Parameters.AddWithValue("@dateEvent", obj.dateEvent);
            cmd.Parameters.AddWithValue("@typeInsurance", obj.typeInsurance);
            cmd.Parameters.AddWithValue("@workShop", obj.workShop);
            cmd.Parameters.AddWithValue("@admins", obj.admin);
            cmd.Parameters.AddWithValue("@driver", obj.driver);
            cmd.Parameters.AddWithValue("@personInvolved", obj.personInvolved);
            cmd.Parameters.AddWithValue("@insuranceAplication", obj.insuranceAplication);
            cmd.Parameters.AddWithValue("@crashCounterPart", obj.crashCounterPart);
            cmd.Parameters.AddWithValue("@OBS", obj.observations);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { respuesta = "Error, " + ex.Message.ToString(); }
            return respuesta;
        }
        public string Actualizar(Sinister obj)
        {
            string respuesta = "ok";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand("update sinisters set idUnit=@idUnit, " +
                "typeSinister=@typeSinister,st1=@st1,st2=@st2,st3=st3," +
                "settlement=@settlement,worl=@WinOrLoose,insurance=@insurance," +
                "dateEvent=@dateEvent,typeInsurance=@typeInsurance," +
                "workShop=@workShop,admins=@admins,driver=@driver,personInvolved=@personInvolved," +
                "insuranceAplication=@insuranceAplication,crashCounterPart=@crashCounterPart,OBS=@observations" +
                " where id=@id", con);
            cmd.Parameters.AddWithValue("@id", obj.id);
            cmd.Parameters.AddWithValue("@idUnit", obj.idUnit);
            cmd.Parameters.AddWithValue("@typeSinister", obj.typeSinister);
            cmd.Parameters.AddWithValue("@st1", obj.st1);
            cmd.Parameters.AddWithValue("@st2", obj.st2);
            cmd.Parameters.AddWithValue("@st3", obj.st3);
            cmd.Parameters.AddWithValue("@settlement", obj.settlement);
            cmd.Parameters.AddWithValue("@WinOrLoose", obj.winOrLoose);
            cmd.Parameters.AddWithValue("@insurance", obj.insurance);
            cmd.Parameters.AddWithValue("@dateEvent", obj.dateEvent);
            cmd.Parameters.AddWithValue("@typeInsurance", obj.typeInsurance);
            cmd.Parameters.AddWithValue("@workShop", obj.workShop);
            cmd.Parameters.AddWithValue("@admins", obj.admin);
            cmd.Parameters.AddWithValue("@driver", obj.driver);
            cmd.Parameters.AddWithValue("@personInvolved", obj.personInvolved);
            cmd.Parameters.AddWithValue("@insuranceAplication", obj.insuranceAplication);
            cmd.Parameters.AddWithValue("@crashCounterPart", obj.crashCounterPart);
            cmd.Parameters.AddWithValue("@observations", obj.observations);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { respuesta = "Error, " + ex.Message.ToString(); }
            return respuesta;
        }
        public string eliminar(int id)
        {
            string respuesta = "ok";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            string cadena = "delete from sinisters where id=@id";
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
