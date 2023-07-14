using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace RESTDLL
{
    public class UserSessions : DBManager
    {

        #region BoringStuff

        string ConnectionString { get; set; }

        MySqlConnection sqlConnection { get; set; }

        public UserSessions(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public UserSessions(MySqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public MySqlConnection GetConnection()
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

        string baseSQL = "SELECT * from usersession as us join user as u on us.userid = u.userid ";

        public List<MDO.RESTDataEntities.Standard.UserSession> GetAllUserSessions()
        {
            List<MDO.RESTDataEntities.Standard.UserSession> p = new List<MDO.RESTDataEntities.Standard.UserSession>();

            var s = GetConnection();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(baseSQL, conn);
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

        public MDO.RESTDataEntities.Standard.UserSession GetUserSessionByID(int userID)
        {
            MDO.RESTDataEntities.Standard.UserSession person = new MDO.RESTDataEntities.Standard.UserSession();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(baseSQL + " Where UserSessionID = @ID;", conn);

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

        public MDO.RESTDataEntities.Standard.UserSession GetUserSessionByToken(string email)
        {
            MDO.RESTDataEntities.Standard.UserSession person = new MDO.RESTDataEntities.Standard.UserSession();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(baseSQL + " Where Token = @ID;", conn);

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

        public List<MDO.RESTDataEntities.Standard.UserSession> GetAllUserSessionByUserID(int userID)
        {
            List<MDO.RESTDataEntities.Standard.UserSession> p = new List<MDO.RESTDataEntities.Standard.UserSession>();

            var s = GetConnection();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(baseSQL + " where us.UserID = @ID;", conn);

                cmd.Parameters.AddWithValue("@ID", userID);

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

        public long? SaveUserSession(MDO.RESTDataEntities.Standard.UserSession p)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = "";

                if (p.UserSessionID == null)
                {
                    sql = @"INSERT INTO `usersession` (`UserSessionID`, `UserID`, `CreatedDate`, `ModifiedDate`, `Token`) VALUES (NULL, @UserID, @CreatedDate, @ModifiedDate, @Token);
                            SELECT LAST_INSERT_ID();";
                }
                else
                {
                    sql = @"UPDATE `usersession` SET UserID = @UserID, CreatedDate = @CreatedDate, ModifiedDate = @ModifiedDate, Token = @Token WHERE UserSessionID = @ID;";
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", p.UserSessionID);
                cmd.Parameters.AddWithValue("@UserID", p.UserID);;
                cmd.Parameters.AddWithValue("@CreatedDate", p.CreatedDate);
                cmd.Parameters.AddWithValue("@ModifiedDate", p.ModifiedDate);
                cmd.Parameters.AddWithValue("@Token", p.Token);

                if (p.UserSessionID == null) //Get new id number
                {
                    return cmd.ExecuteScalar().ToString().ConvertToNullableInt();
                }
                else //Return id if worked
                {
                    cmd.ExecuteScalar();

                    return p.UserSessionID;
                }
            }
        }

        internal MDO.RESTDataEntities.Standard.UserSession ConvertMySQLToEntity(MySqlDataReader reader)
        {
            MDO.RESTDataEntities.Standard.UserSession p = new MDO.RESTDataEntities.Standard.UserSession();

            p.UserSessionID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "UserSessionID"));
            p.UserID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "UserID"));
            p.CreatedDate = DBUtilities.ReturnSafeDateTime(reader, "CreatedDate").Value;
            p.ModifiedDate = DBUtilities.ReturnSafeDateTime(reader, "ModifiedDate").Value;
            p.Token = DBUtilities.ReturnSafeString(reader, "Token");
            p.Username = DBUtilities.ReturnSafeString(reader, "Username");

            return p;
        }
    }
}
