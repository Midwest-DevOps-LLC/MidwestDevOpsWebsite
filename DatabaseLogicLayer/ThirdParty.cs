using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DatabaseLogicLayer
{
    public class ThirdParty : DBManager
    {

        #region BoringStuff

        string ConnectionString { get; set; }

        MySqlConnection sqlConnection { get; set; }

        public ThirdParty(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public ThirdParty(MySqlConnection sqlConnection)
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

        public List<DataEntities.ThirdParty> GetAllThirdParties()
        {
            List<DataEntities.ThirdParty> p = new List<DataEntities.ThirdParty>();

            var s = GetConnection();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM thirdparty", conn);
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

        public DataEntities.ThirdParty GetThirdPartyByID(int thirdPartyID)
        {
            DataEntities.ThirdParty person = new DataEntities.ThirdParty();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM thirdparty Where thirdpartyid = @ID;", conn);

                cmd.Parameters.AddWithValue("@ID", thirdPartyID);
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

        public DataEntities.ThirdParty GetThirdPartyByName(string name)
        {
            DataEntities.ThirdParty person = new DataEntities.ThirdParty();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM thirdparty Where Name = @Username;", conn);

                cmd.Parameters.AddWithValue("@Username", name);
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

        public long? SaveThirdParty(DataEntities.ThirdParty p)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = "";

                if (p.ThirdPartyID == null)
                {
                    sql = @"INSERT INTO `thirdparty` (`ThirdPartyID`, `Name`, `IconUrl`, `AuthorizeUrl`, `Description`) VALUES (NULL, @Name, @IconUrl, @AuthorizeUrl, @Description);
                            SELECT LAST_INSERT_ID();";
                }
                else
                {
                    sql = @"UPDATE `thirdparty` SET Name = @Name, IconUrl = @IconUrl, AuthorizeUrl = @AuthorizeUrl, Description = @Description WHERE ThirdPartyID = @ID;";
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", p.ThirdPartyID);
                cmd.Parameters.AddWithValue("@Name", p.Name);
                cmd.Parameters.AddWithValue("@IconUrl", p.IconUrl);
                cmd.Parameters.AddWithValue("@AuthorizeUrl", p.AuthorizeUrl);
                cmd.Parameters.AddWithValue("@Description", p.Description);

                if (p.ThirdPartyID == null) //Get new id number
                {
                    return cmd.ExecuteScalar().ToString().ConvertToNullableInt();
                }
                else //Return id if worked
                {
                    cmd.ExecuteScalar();

                    return p.ThirdPartyID;
                }
            }
        }

        internal DataEntities.ThirdParty ConvertMySQLToEntity(MySqlDataReader reader)
        {
            DataEntities.ThirdParty p = new DataEntities.ThirdParty();

            p.ThirdPartyID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "ThirdPartyID"));
            p.Name = DBUtilities.ReturnSafeString(reader, "Name");
            p.IconUrl = DBUtilities.ReturnSafeString(reader, "IconUrl");
            p.AuthorizeUrl = DBUtilities.ReturnSafeString(reader, "AuthorizeUrl");
            p.Description = DBUtilities.ReturnSafeString(reader, "Description");

            return p;
        }
    }
}
