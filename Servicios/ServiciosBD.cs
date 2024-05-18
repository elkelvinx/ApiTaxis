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
        public static SqlConnection ObtenerConexion(string usuario, string contraseña)
        {
            usuario = "siniestros";
            contraseña = "123";
            SqlConnection conexion = new SqlConnection();
            string cadena = $"server=LAPTOP-2LUPI6JF\\SQLEXPRESS; database=TaxisBdTeo; User Id={usuario}; Password={contraseña};";
            conexion.ConnectionString = cadena;
            try
            {
                conexion.Open();
            }
            catch (SqlException ex)
            {
                // Manejar el error de SQL aquí
                throw;
            }
            return conexion;
        }
        public static SqlConnection ObtenerConexion2(string usuario, string contraseña)
        {
            // Asegúrate de validar y sanear las entradas 'usuario' y 'contraseña'
            // ...

            SqlConnection conexion = new SqlConnection();
            // Utiliza parámetros parametrizados para la cadena de conexión
            string cadena = "server=LAPTOP-2LUPI6JF\\SQLEXPRESS; database=TaxisBdTeo; User Id=@usuario; Password=@contraseña;";
            conexion.ConnectionString = cadena;

            // Agrega los parámetros al objeto de conexión
            /*
                 conexion.Parameters.AddWithValue("@usuario", usuario);
                conexion.Parameters.AddWithValue("@contraseña", contraseña);
                conexion.
             */
            try
            {
                conexion.Open();
            }
            catch (SqlException ex)
            {
                // Manejar el error de SQL aquí
                throw;
            }

            return conexion;
        }


    }
}
