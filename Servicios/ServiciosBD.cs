using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Text;


namespace Servicios
{
    public class ServiciosBD
    {

        public static SqlConnection ObtenerConexion()
        {
            // Si estás debuggeando en tu PC → usa LocalConnection
            string connectionStringName = EsDesarrolloLocal()
                ? "LocalConnection"
                : "DefaultConnection";

            string cadena = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            var conexion = new SqlConnection(cadena);
            try
            {
                conexion.Open();
                return conexion;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error conectando a la base de datos ({connectionStringName}): {ex.Message}", ex);
            }
        }

        private static bool EsDesarrolloLocal()
        {
            // Detecta si estás corriendo en tu máquina
            return System.Diagnostics.Debugger.IsAttached ||
                   Environment.MachineName.Equals("LAPTOP-2LUPI6JF", StringComparison.OrdinalIgnoreCase);
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
