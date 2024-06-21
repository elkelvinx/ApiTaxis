using Entidades;
using Entidades.Arrays;
using Entidades.LogIn;
using Entidades.Response;
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
            /*  response=<int>
                response2=<string>  
            crear el usuario y obtener su id, para el permissions */
            var responseUSer = InsertarUser(obj.User);
            if(responseUSer.Success == false)
            {
                throw new Exception(responseUSer.ErrorMessage);
            }
            //insertar permisos al usuario
            obj.Permissions.IdUser = responseUSer.Data;
            var responsePermission = InsertarUserPermission(obj.Permissions);
            if (responsePermission.Success == false)
            {
                throw new Exception(responseUSer.ErrorMessage);
            }
            return "ok";
        }
        public ApiResponse<int> InsertarUser(User obj)
        {
            //devolvera id o error
            var response = new ApiResponse<int>();
            //encriptacion de datos
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
                    cmd.Parameters.AddWithValue("@dateCreated", DateTime.UtcNow);
                    try
                    {  
                        con.Open();
                        //recibimos el id del usuario nuevo
                        response.Data = Convert.ToInt32(cmd.ExecuteScalar());
                       //seria necesario de no estar en la cadena cmd.Transaction.Commit();
                        response.Success= true;
                    }
                    catch (SqlException ex)
                    {
                        cmd.Transaction.Rollback();
                        response.Success = false;
                        response.ErrorMessage = "Error SQL: " + ex.Message;
                    }
                }
            }
            return response;
        }
        public ApiResponse<string> InsertarUserPermission(UserPermissions obj)
        {
            var response = new ApiResponse<string>();
            string cadena ="BEGIN TRANSACTION; " +
            "INSERT INTO userPermissions (idUser, idRole, driver, admin, permissionair, unit, sinister, extraData, pdf) " +
            "VALUES (@idUser, @idRole, @driver, @admin, @permissionair, @unit, @sinister, @extraData, @pdf); " +
            "COMMIT;";
            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(cadena, con))
                {
                    cmd.Parameters.AddWithValue("@idUser", obj.IdUser);
                    cmd.Parameters.AddWithValue("@IdRole", obj.IdRole);
                    cmd.Parameters.AddWithValue("@driver", obj.Driver);
                    cmd.Parameters.AddWithValue("@admin", obj.Admin);
                    cmd.Parameters.AddWithValue("@permissionair", obj.Permissionaire);
                    cmd.Parameters.AddWithValue("@unit", obj.Unit);
                    cmd.Parameters.AddWithValue("@sinister", obj.Sinister);
                    cmd.Parameters.AddWithValue("@extraData", obj.ExtraData);
                    cmd.Parameters.AddWithValue("@pdf", obj.Pdf);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        response.Data = "ok";
                        response.Success = true;
                    }
                    catch (SqlException ex)
                    {
                        cmd.Transaction.Rollback();
                        response.Success = false;
                        response.ErrorMessage = "Error SQL: " + ex.Message;
                    }
                }
            }
            return response;
        }
    }
}
