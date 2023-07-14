using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DatabaseLogicLayer
{
    public class ThirdPartyUser : DBManager
    {

        #region BoringStuff

        string ConnectionString { get; set; }

        MySqlConnection sqlConnection { get; set; }

        public ThirdPartyUser(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public ThirdPartyUser(MySqlConnection sqlConnection)
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

        public List<DataEntities.ThirdPartyUser> GetAllThirdPartyUsers()
        {
            List<DataEntities.ThirdPartyUser> p = new List<DataEntities.ThirdPartyUser>();

            var s = GetConnection();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM thirdpartyuser", conn);
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

        public List<DataEntities.ThirdPartyUser> GetAllThirdPartyUsersByUserID(int userID)
        {
            List<DataEntities.ThirdPartyUser> p = new List<DataEntities.ThirdPartyUser>();

            var s = GetConnection();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM thirdpartyuser where userid = @ID", conn);

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

        public DataEntities.ThirdPartyUser GetThirdPartyUserByID(int thirdPartyUserID)
        {
            DataEntities.ThirdPartyUser person = new DataEntities.ThirdPartyUser();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM thirdpartyuser Where thirdpartyuserid = @ID;", conn);

                cmd.Parameters.AddWithValue("@ID", thirdPartyUserID);
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

        public long? SaveThirdPartyUser(DataEntities.ThirdPartyUser p)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = "";

                if (p.ThirdPartyUserID == null)
                {
                    sql = @"INSERT INTO `thirdpartyuser` (`ThirdPartyUserID`, `ThirdPartyID`, `UserID`, `ApiKey`) VALUES (NULL, @ThirdPartyID, @UserID, @ApiKey);
                            SELECT LAST_INSERT_ID();";
                }
                else
                {
                    sql = @"UPDATE `thirdpartyuser` SET ThirdPartyID = @ThirdPartyID, UserID = @UserID, ApiKey = @ApiKey WHERE ThirdPartyUserID = @ID;";
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", p.ThirdPartyUserID);
                cmd.Parameters.AddWithValue("@ThirdPartyID", p.ThirdPartyID);
                cmd.Parameters.AddWithValue("@UserID", p.UserID);
                cmd.Parameters.AddWithValue("@ApiKey", p.ApiKey);

                if (p.ThirdPartyUserID == null) //Get new id number
                {
                    return cmd.ExecuteScalar().ToString().ConvertToNullableInt();
                }
                else //Return id if worked
                {
                    cmd.ExecuteScalar();

                    return p.ThirdPartyUserID;
                }
            }
        }

        internal DataEntities.ThirdPartyUser ConvertMySQLToEntity(MySqlDataReader reader)
        {
            DataEntities.ThirdPartyUser p = new DataEntities.ThirdPartyUser();

            p.ThirdPartyUserID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "ThirdPartyUserID"));
            p.ThirdPartyID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "ThirdPartyID"));
            p.UserID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "UserID"));
            p.ApiKey = DBUtilities.ReturnSafeString(reader, "ApiKey");

            return p;
        }
    }
}
