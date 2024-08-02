using Entidades.Arrays;
using Entidades.LogIn;
using ServicioEncriptacion;
using System;
using System.Data.SqlClient;
using System.Transactions;

namespace Servicios
{
    public class ServicioUser
    {
        private readonly Encriptacion _encriptacion;
        public ServicioUser()
        {
            _encriptacion = new Encriptacion();
        }
        public ApiResponse<listUsersData> ConsultarUsers()
        {
            var data = new ApiResponse<listUsersData>();
            var list = new listUsersData();
            try
            {
                using (SqlConnection con = ServiciosBD.ObtenerConexion())
                {
                    string cadena = "SELECT d.name, d.email, d.dateCreated, d.dateOut,d.active, " +
                                    "p.* FROM UsersData AS d " +
                                    "INNER JOIN UserPermissions AS p ON p.idUser = d.id";
                    using (SqlCommand command = new SqlCommand(cadena, con))

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var obj = new UserData
                            {
                                User = new User
                                {
                                    id = Convert.ToInt32(reader["idUser"]),
                                    name = reader["name"].ToString(),
                                    email = reader["email"].ToString(),
                                    dateCreated = Convert.ToDateTime(reader["dateCreated"]),
                                    //CORREGIR
                                    dateOut = reader["dateOut"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["dateOut"]),
                                    active = bool.Parse(reader["active"].ToString())
                                },
                                Permissions = new UserPermissions
                                {
                                    id = Convert.ToInt32(reader["id"]),
                                    idUser = Convert.ToInt32(reader["idUser"]),
                                    idRole = Convert.ToInt32(reader["idRole"]),
                                    driver = Convert.ToBoolean(reader["driver"]),
                                    admin = Convert.ToBoolean(reader["admin"]),
                                    permissionaire = Convert.ToBoolean(reader["permissionair"]),
                                    unit = Convert.ToBoolean(reader["unit"]),
                                    sinister = Convert.ToBoolean(reader["sinister"]),
                                    extraData = Convert.ToBoolean(reader["extraData"]),
                                    changeLog= Convert.ToBoolean(reader["changeLog"]),
                                    pdf = Convert.ToBoolean(reader["pdf"])
                                }
                            };

                            list.Add(obj);
                        }
                    }
                    SqlConnection.ClearPool(con);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                data.ErrorMessage = ex.Message;
                data.Success = false;
                return data;
            }
            data.Data = list;
            data.Success = true;
            return data;
        }
        public string Insertar(UserData obj)
        {
            /*  response=<int>
                response2=<string>  
            crear el usuario y obtener su id, para el permissions */
            var responseUSer = VerifyUser(obj.User.name);
            if (responseUSer.Success == false)
            {
                throw new Exception(responseUSer.ErrorMessage);
            }
            responseUSer = InsertarUser(obj.User);
            if (responseUSer.Success == false)
            {
                throw new Exception(responseUSer.ErrorMessage);
            }
            //insertar permisos al usuario
            obj.Permissions.idUser = responseUSer.Data;
            var responsePermission = InsertarUserPermission(obj.Permissions);
            if (responsePermission.Success == false)
            {
                throw new Exception(responseUSer.ErrorMessage);
            }
            return "ok";
        }
        public ApiResponse<int> VerifyUser(string nameUser)
        {
            var response = new ApiResponse<int>();
            string cadena = "select id from usersData where name= @name";
            SqlConnection con = ServiciosBD.ObtenerConexion();
            SqlCommand command = new SqlCommand(cadena, con);
            command.Parameters.AddWithValue("@name", nameUser);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                response.Success = false;
                response.ErrorMessage = "El nombre de usuario ya esta en uso, porfavor escoga otro. ";
            }
            else
                response.Success = true;
            return response;
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
                    cmd.Parameters.AddWithValue("@active", 1);
                    try
                    {
                        //con.Open();
                        //recibimos el id del usuario nuevo
                        response.Data = Convert.ToInt32(cmd.ExecuteScalar());
                        //seria necesario de no estar en la cadena cmd.Transaction.Commit();
                        con.Close();
                        response.Success = true;
                    }
                    catch (SqlException ex)
                    {
                        cmd.Transaction.Rollback();
                        response.Success = false;
                        response.ErrorMessage = "Error SQL: " + ex.Message;
                    }
                }
                SqlConnection.ClearPool(con);
            }
            return response;
        }
        public ApiResponse<string> InsertarUserPermission(UserPermissions obj)
        {
            var response = new ApiResponse<string>();
            string cadena = "BEGIN TRANSACTION; " +
            "INSERT INTO userPermissions (idUser, idRole, driver, admin, permissionair, unit, sinister, extraData, pdf) " +
            "VALUES (@idUser, @idRole, @driver, @admin, @permissionair, @unit, @sinister, @extraData, @pdf); " +
            "COMMIT;";
            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(cadena, con))
                {
                    cmd.Parameters.AddWithValue("@idUser", obj.idUser);
                    cmd.Parameters.AddWithValue("@IdRole", obj.idRole);
                    cmd.Parameters.AddWithValue("@driver", obj.driver);
                    cmd.Parameters.AddWithValue("@admin", obj.admin);
                    cmd.Parameters.AddWithValue("@permissionair", obj.permissionaire);
                    cmd.Parameters.AddWithValue("@unit", obj.unit);
                    cmd.Parameters.AddWithValue("@sinister", obj.sinister);
                    cmd.Parameters.AddWithValue("@extraData", obj.extraData);
                    cmd.Parameters.AddWithValue("@pdf", obj.pdf);
                    try
                    {
                        //con.Open();
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
                SqlConnection.ClearPool(con);
            }
            return response;
        }
        public ApiResponse<string> Actualizar(UserData obj)
        {
            var response = new ApiResponse<string>();
            response.Data = "ok";

            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            {
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    SqlCommand cmdUserData = new SqlCommand("UPDATE usersData SET name=@name," +
                        "email=@email,active=@active,password=@password,dateOut=@dateOut,bloqued=@bloqued WHERE id=@id", con, transaction);
                    cmdUserData.Parameters.AddWithValue("@id", obj.User.id);
                    cmdUserData.Parameters.AddWithValue("@name", obj.User.name);
                    cmdUserData.Parameters.AddWithValue("@email", obj.User.email);
                    cmdUserData.Parameters.AddWithValue("@active", obj.User.active);
                    cmdUserData.Parameters.AddWithValue("@bloqued", obj.User.bloqued);
                    if (obj.User.active == true)
                        cmdUserData.Parameters.AddWithValue("@dateOut", DateTime.Parse("1800-01-01T00:00:00"));
                    //en caso de que la fecha sea default y tenga que poner la actual
                    else if (obj.User.dateOut >= DateTime.Parse("1800-01-01T00:00:00"))
                     cmdUserData.Parameters.AddWithValue("@dateOut", DateTime.Now);
                   //if dateOut ya tiene una fecha real
                    else
                        cmdUserData.CommandText = cmdUserData.CommandText.Replace(",dateOut=@dateOut", "");
                    //password verification
                    if (string.IsNullOrEmpty(obj.User.password))
                        cmdUserData.CommandText = cmdUserData.CommandText.Replace(",password=@password", "");
                    else
                    {
                        obj.User.password = _encriptacion.GetSHA256(obj.User.password);
                        cmdUserData.Parameters.AddWithValue("@password", obj.User.password);
                    }
                    cmdUserData.ExecuteNonQuery();  
                    SqlCommand cmdUserPermissions = new SqlCommand("UPDATE userPermissions SET idRole=@idRole, " +
                        "driver=@driver, admin=@admin, permissionair=@permissionair, unit=@unit, " +
                        "sinister=@sinister, extraData=@extraData, changeLog=@changeLog, pdf=@pdf WHERE idUser=@idUser", con, transaction);
                    cmdUserPermissions.Parameters.AddWithValue("@idUser", obj.User.id);
                    cmdUserPermissions.Parameters.AddWithValue("@idRole", obj.Permissions.idRole);
                    cmdUserPermissions.Parameters.AddWithValue("@driver", obj.Permissions.driver);
                    cmdUserPermissions.Parameters.AddWithValue("@admin", obj.Permissions.admin);
                    cmdUserPermissions.Parameters.AddWithValue("@permissionair", obj.Permissions.permissionaire);
                    cmdUserPermissions.Parameters.AddWithValue("@unit", obj.Permissions.unit);
                    cmdUserPermissions.Parameters.AddWithValue("@sinister", obj.Permissions.sinister);
                    cmdUserPermissions.Parameters.AddWithValue("@extraData", obj.Permissions.extraData);
                    cmdUserPermissions.Parameters.AddWithValue("@changeLog", obj.Permissions.changeLog);
                    cmdUserPermissions.Parameters.AddWithValue("@pdf", obj.Permissions.pdf);
                    cmdUserPermissions.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Success = false;
                    response.ErrorMessage = "Error: " + ex.Message;
                }
            }
            response.Success = true;
            return response;
        }
        public ApiResponse<string> softDelete(int id)
        {
            var response = new ApiResponse<string>();
            response.Data = "ok";
            using (SqlConnection con = ServiciosBD.ObtenerConexion())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("update userData set active=false where id=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    response.Success = true;
                    return response;
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.ErrorMessage = "Error: " + ex.Message;
                }
            }
            return response;
        }
    }
}