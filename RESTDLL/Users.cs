using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace RESTDLL
{
    public class Users : DBManager
    {

        #region BoringStuff

        string ConnectionString { get; set; }

        MySqlConnection sqlConnection { get; set; }

        public Users(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public Users(MySqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        internal MySqlConnection GetConnection()
        {
            if (sqlConnection == null)
            {
                return new MySqlConnection(this.ConnectionString);
            }
            else
            {
                return this.sqlConnection;
            }
        }

        #endregion

        public List<MDO.RESTDataEntities.Standard.User> GetAllUsers()
        {
            List<MDO.RESTDataEntities.Standard.User> p = new List<MDO.RESTDataEntities.Standard.User>();

            var s = GetConnection();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM user", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        p.Add(ConvertMySQLToEntity(reader));
                    }
                }
            }

            return p;
        }

        public MDO.RESTDataEntities.Standard.User GetUserByID(int userID)
        {
            MDO.RESTDataEntities.Standard.User person = new MDO.RESTDataEntities.Standard.User();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM user Where UserID = @ID;", conn);

                cmd.Parameters.AddWithValue("@ID", userID);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return ConvertMySQLToEntity(reader);
                    }

                    return null;
                }
            }
        }

        public MDO.RESTDataEntities.Standard.User GetUserByEmail(string email)
        {
            MDO.RESTDataEntities.Standard.User person = new MDO.RESTDataEntities.Standard.User();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM user Where Email = @ID;", conn);

                cmd.Parameters.AddWithValue("@ID", email);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return ConvertMySQLToEntity(reader);
                    }

                    return null;
                }
            }
        }

        public MDO.RESTDataEntities.Standard.User GetUserByUsername(string userName)
        {
            MDO.RESTDataEntities.Standard.User person = new MDO.RESTDataEntities.Standard.User();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM user Where Username = @Username;", conn);

                cmd.Parameters.AddWithValue("@Username", userName);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return ConvertMySQLToEntity(reader);
                    }

                    return null;
                }
            }
        }

        public MDO.RESTDataEntities.Standard.User GetUserByUUID(string UUID)
        {
            MDO.RESTDataEntities.Standard.User person = new MDO.RESTDataEntities.Standard.User();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM user Where UUID = @UUID;", conn);

                cmd.Parameters.AddWithValue("@UUID", UUID);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return ConvertMySQLToEntity(reader);
                    }

                    return null;
                }
            }
        }

        public long? SaveUser(MDO.RESTDataEntities.Standard.User p)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = "";

                if (p.UserID == null)
                {
                    sql = @"INSERT INTO `user` (`UserID`, `Username`, `FirstName`, `MiddleName`, `LastName`, `Email`, `Password`, `CreatedBy`, `CreatedDate`, `ModifiedBy`, `ModifiedDate`, `Active`, `Activated`) VALUES (NULL, @Username, @FirstName, @MiddleName, @LastName, @Email, @Password, @CreatedBy, @CreatedDate, @ModifiedBy, @ModifiedDate, @Active, @Activated);
                            SELECT LAST_INSERT_ID();";
                }
                else
                {
                    sql = @"UPDATE `user` SET UserID = @ID, Username = @Username, FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName, Email = @Email, Password = @Password, CreatedBy = @CreatedBy, CreatedDate = @CreatedDate, ModifiedBy = @ModifiedBy, ModifiedDate = @ModifiedDate, Active = @Active, Activated = @Activated WHERE UserID = @ID;";
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", p.UserID);
                cmd.Parameters.AddWithValue("@Username", p.Username.ToLower());
                cmd.Parameters.AddWithValue("@FirstName", p.FirstName);
                cmd.Parameters.AddWithValue("@MiddleName", p.MiddleName);
                cmd.Parameters.AddWithValue("@LastName", p.LastName);
                cmd.Parameters.AddWithValue("@Email", p.Email);
                cmd.Parameters.AddWithValue("@Password", p.Password);
                cmd.Parameters.AddWithValue("@CreatedBy", p.CreatedBy);
                cmd.Parameters.AddWithValue("@CreatedDate", p.CreatedDate);
                cmd.Parameters.AddWithValue("@ModifiedBy", p.ModifiedBy);
                cmd.Parameters.AddWithValue("@ModifiedDate", p.ModifiedDate);
                cmd.Parameters.AddWithValue("@Active", p.Active);
                cmd.Parameters.AddWithValue("@Activated", p.Activated);



                if (p.UserID == null) //Get new id number
                {
                    return cmd.ExecuteScalar().ToString().ConvertToNullableInt();
                }
                else //Return id if worked
                {
                    cmd.ExecuteScalar();

                    return p.UserID;
                }
            }
        }

        internal MDO.RESTDataEntities.Standard.User ConvertMySQLToEntity(MySqlDataReader reader)
        {
            MDO.RESTDataEntities.Standard.User p = new MDO.RESTDataEntities.Standard.User();

            p.UserID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "UserID"));
            p.UUID = DBUtilities.ReturnSafeString(reader, "UUID");
            p.Username = DBUtilities.ReturnSafeString(reader, "Username");
            p.FirstName = DBUtilities.ReturnSafeString(reader, "FirstName");
            p.MiddleName = DBUtilities.ReturnSafeString(reader, "MiddleName");
            p.LastName = DBUtilities.ReturnSafeString(reader, "LastName");
            p.Email = DBUtilities.ReturnSafeString(reader, "Email");
            p.Password = DBUtilities.ReturnSafeString(reader, "Password");
            p.Active = DBUtilities.ReturnBoolean(reader, "Active");
            p.Activated = DBUtilities.ReturnBoolean(reader, "Activated");
            p.CreatedBy = DBUtilities.ReturnSafeInt(reader, "CreatedBy");
            p.CreatedDate = DBUtilities.ReturnSafeDateTime(reader, "CreatedDate");
            p.ModifiedBy = DBUtilities.ReturnSafeInt(reader, "ModifiedBy");
            p.ModifiedDate = DBUtilities.ReturnSafeDateTime(reader, "ModifiedDate");

            return p;
        }
    }
}
