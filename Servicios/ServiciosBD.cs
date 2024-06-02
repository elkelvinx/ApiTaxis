using System.Data.SqlClient;
using System;
using System.Data;
using System.Text;
using System.Configuration;

namespace Servicios
{
    public class ServiciosBD
    {
        public static SqlConnection ObtenerConexion()
        {

            SqlConnection conexion = new SqlConnection();

            string cadena = "server=LAPTOP-2LUPI6JF\\SQLEXPRESS; database=TaxisBdTeo; integrated security = true";
            conexion.ConnectionString = cadena;
            try
            {
                conexion.Open();
            }
            catch (Exception)
            {
                throw;
            }

            return conexion;

        }
        //cosas del profe/////////////////////
        public static SqlCommand ObtenerComando(SqlConnection conn, string sql)
        {
            return new SqlCommand(sql, conn);
        }

        public bool EjecutarProcedimiento(string pNombre)
        {
            SqlConnection conexion = ObtenerConexion();
            try
            {
                SqlCommand cmdProc = new SqlCommand(pNombre, conexion);
                cmdProc.CommandType = CommandType.StoredProcedure;
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                cmdProc.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.Close();
                SqlConnection.ClearPool(conexion);
            }
        }

        public DateTime ObtenerFechaServidor()
        {
            SqlConnection conexion = ServiciosBD.ObtenerConexion();
            try
            {
                StringBuilder sentencia = new StringBuilder("SELECT GETDATE() Fecha");
                SqlCommand comando = ServiciosBD.ObtenerComando(conexion, sentencia.ToString());
                object resultado = comando.ExecuteScalar();
                if (resultado != null && resultado.GetType() != typeof(DBNull))
                {
                    return Convert.ToDateTime(resultado);
                }
                else
                {
                    return DateTime.Today;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conexion.Close();
                SqlConnection.ClearPool(conexion);
            }
        }


    }
}
