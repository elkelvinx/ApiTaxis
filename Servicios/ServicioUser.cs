using Entidades;
using Entidades.LogIn;
using EO.WebBrowser;
using ServicioEncriptacion;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace Servicios
{
    public class ServicioUser
    {
        private readonly Encriptacion _encriptacion;
        public ServicioUser()
        {
            _encriptacion = new Encriptacion();
        }
        public listUsersData ConsultarUsers()
        {
            string cadena = "select d.name,d.email,d.dateCreated,d.dateOut," +
                "p.* from UsersData as d Inner Join UserPermissions as p On p.idUser= d.id ";            
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand command = new SqlCommand(cadena, con);
            SqlDataReader reader = command.ExecuteReader();
            listUsersData list = new listUsersData();
            while (reader.Read())
            {
                UserData obj = new UserData();
                obj.User = new User(); // Inicializa el objeto User
                obj.Permissions = new UserPermissions(); // Inicializa el objeto UserPermissions¿
                obj.User.name= reader["name"].ToString();
                obj.User.email = reader["email"].ToString();
                obj.User.dateCreated = DateTime.Parse(reader["dateCreated"].ToString());
                try { obj.User.dateOut = DateTime.Parse(reader["dateOut"].ToString()); }
                catch { obj.User.dateOut = new DateTime(1900, 1, 1); }
                obj.Permissions.IdRole = Int16.Parse(reader["idRole"].ToString());
                obj.Permissions.Driver = bool.Parse(reader["driver"].ToString());
                obj.Permissions.Admin = bool.Parse(reader["admin"].ToString());
                obj.Permissions.Permissionaire = bool.Parse(reader["permissionair"].ToString());
                obj.Permissions.Unit = bool.Parse(reader["unit"].ToString());
                obj.Permissions.Sinister = bool.Parse(reader["sinister"].ToString());                
                obj.Permissions.ExtraData = bool.Parse(reader["extraData"].ToString());
                obj.Permissions.Pdf = bool.Parse(reader["pdf"].ToString());
                obj.User.id = Int16.Parse(reader["idUser"].ToString());
                obj.Permissions.Id = Int16.Parse(reader["id"].ToString());
                list.Add(obj);
                SqlConnection.ClearPool(con);
            }
            SqlConnection.ClearPool(con);
            con.Close();
            
            return list;
        }
        public string Insertar(UserData obj)
        {
            string response = "ok";
            response = InsertarUser(obj.User);
            //crear el usuario y obtener su id, para el permissions
            response = InsertarUserPermission(obj.Permissions);
            try
            {
                return "ok";
            }
            catch (SqlException ex)
            {
                throw new Exception("Error SQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public string InsertarUser(User obj)
        {
            string response = "ok";
            obj.password = _encriptacion.GetSHA256(obj.password);
            //transaction que inserta un User y obtiene el id de ese User
            string cadena = "BEGIN TRANSACTION; " +
                         "INSERT INTO usersData (name, email, password, dateCreated) " +
                         "VALUES (@name, @email, @password, @dateCreated); " +
                         "SELECT SCOPE_IDENTITY(); " +
                         "COMMIT;";
            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(cadena, con))
                {
                    cmd.Parameters.AddWithValue("@name", obj.name);
                    cmd.Parameters.AddWithValue("@email", obj.email);
                    cmd.Parameters.AddWithValue("@password", obj.password);
                    cmd.Parameters.AddWithValue("@dateCreated", DateTime.Now);
                    try
                    {
                        con.Open();
                        int userId = Convert.ToInt32(cmd.ExecuteScalar());
                        con.Close();
                        return response;
                    }
                    catch (SqlException ex)
                    {
                        con.Close();
                        throw new Exception("Error SQL: " + ex.Message);
                    }
                }
            }
        }
        public string InsertarUserPermission(UserPermissions obj)
        {
            string cadena = " ";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand cmd = new SqlCommand(cadena, con);
            cmd.Parameters.AddWithValue("@idUser", obj.Id);
            cmd.Parameters.AddWithValue("@st1", obj.IdUser);
            cmd.Parameters.AddWithValue("@st2", obj.IdRole);
            cmd.Parameters.AddWithValue("@st3", obj.Driver);
            cmd.Parameters.AddWithValue("@extNumber", obj.Admin);
            cmd.Parameters.AddWithValue("@birth", obj.Permissionaire);
            cmd.Parameters.AddWithValue("@password", obj.Unit);
            cmd.Parameters.AddWithValue("@admin", obj.Sinister);
            cmd.Parameters.AddWithValue("@licenseEx", obj.ExtraData);
            cmd.Parameters.AddWithValue("@ingressPay", obj.Pdf);
            try
            {
                cmd.ExecuteNonQuery();
                SqlConnection.ClearPool(con);
                con.Close();
                return "ok";
            }
            catch (SqlException ex)
            {
                SqlConnection.ClearPool(con);
                con.Close();
                throw new Exception("Error SQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                SqlConnection.ClearPool(con);
                con.Close();
                throw new Exception("Error: " + ex.Message);
            }
        }
    }
}
