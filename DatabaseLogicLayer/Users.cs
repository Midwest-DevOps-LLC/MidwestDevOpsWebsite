using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DatabaseLogicLayer
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

        public List<DataEntities.User> GetAllUsers()
        {
            List<DataEntities.User> p = new List<DataEntities.User>();

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

        public DataEntities.User GetUserByID(int userID)
        {
            DataEntities.User person = new DataEntities.User();

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

        public DataEntities.User GetUserByEmail(string email)
        {
            DataEntities.User person = new DataEntities.User();

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

        public DataEntities.User GetUserByUsername(string userName)
        {
            DataEntities.User person = new DataEntities.User();

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

        public DataEntities.User GetUserByUUID(string UUID)
        {
            DataEntities.User person = new DataEntities.User();

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

        public long? SaveUser(DataEntities.User p)
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
                cmd.Parameters.AddWithValue("@Email", p.Email);
                cmd.Parameters.AddWithValue("@FirstName", p.FirstName);
                cmd.Parameters.AddWithValue("@MiddleName", p.MiddleName);
                cmd.Parameters.AddWithValue("@LastName", p.LastName);
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

        internal DataEntities.User ConvertMySQLToEntity(MySqlDataReader reader)
        {
            DataEntities.User p = new DataEntities.User();

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
