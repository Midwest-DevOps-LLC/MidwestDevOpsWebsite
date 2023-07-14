using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace RESTDLL
{
    public class Sites : DBManager
    {

        #region BoringStuff

        string ConnectionString { get; set; }

        MySqlConnection sqlConnection { get; set; }

        public Sites(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public Sites(MySqlConnection sqlConnection)
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

        string baseSQL = "SELECT * from site ";

        public List<MDO.RESTDataEntities.Standard.Site> GetAllSites(bool? active)
        {
            List<MDO.RESTDataEntities.Standard.Site> p = new List<MDO.RESTDataEntities.Standard.Site>();

            var s = GetConnection();

            string sql = baseSQL;

            if (active.HasValue)
            {
                if (active.Value == true)
                    sql += " where Active = 1;";
                else
                    sql += " where Active = 0;";
            }

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
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

        public MDO.RESTDataEntities.Standard.Site GetSiteByID(int siteID)
        {
            MDO.RESTDataEntities.Standard.Site person = new MDO.RESTDataEntities.Standard.Site();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(baseSQL + " Where SiteID = @ID;", conn);

                cmd.Parameters.AddWithValue("@ID", siteID);
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

        public MDO.RESTDataEntities.Standard.Site GetSiteByGUID(string email)
        {
            MDO.RESTDataEntities.Standard.Site person = new MDO.RESTDataEntities.Standard.Site();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(baseSQL + " Where GUID = @ID;", conn);

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

        public MDO.RESTDataEntities.Standard.SiteUser GetSiteUserBySiteIDAndUserID(int siteID, int userID)
        {
            MDO.RESTDataEntities.Standard.SiteUser person = new MDO.RESTDataEntities.Standard.SiteUser();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from siteuser " + " Where SiteID = @ID and userID = @UserID;", conn);

                cmd.Parameters.AddWithValue("@ID", siteID);
                cmd.Parameters.AddWithValue("@UserID", userID);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return ConvertSiteUserMySQLToEntity(reader);
                    }

                    return null;
                }
            }
        }

        public List<MDO.RESTDataEntities.Standard.Site> GetAllSitesByUserID(int userID, bool? active)
        {
            List<MDO.RESTDataEntities.Standard.Site> p = new List<MDO.RESTDataEntities.Standard.Site>();

            var s = GetConnection();

            string sql = "SELECT * from site where siteid IN (select su.SiteID from siteuser as su where su.UserID = @ID)";

            if (active.HasValue)
            {
                if (active.Value == true)
                    sql += " AND Active = 1 ;";
                else
                    sql += " AND Active = 0 ;";
            }
            else
                sql += " ";

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);

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

        public long? SaveSite(MDO.RESTDataEntities.Standard.Site p)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = "";

                if (p.SiteID == null)
                {
                    sql = @"INSERT INTO `site` (`SiteID`, `GUID`, `Name`, `Active`, `RegisterSuccessPath`, `URL`) VALUES (NULL, @GUID, @Name, @Active, @RegisterSuccessPath, @URL);
                            SELECT LAST_INSERT_ID();";
                }
                else
                {
                    sql = @"UPDATE `site` SET GUID = @GUID, Name = @Name, Active = @Active, RegisterSuccessPath = @RegisterSuccessPath, URL = @URL WHERE SiteID = @ID;";
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", p.SiteID);
                cmd.Parameters.AddWithValue("@GUID", p.GUID);;
                cmd.Parameters.AddWithValue("@Name", p.Name);
                cmd.Parameters.AddWithValue("@Active", p.Active);
                cmd.Parameters.AddWithValue("@RegisterSuccessPath", p.RegisterSuccessPath);
                cmd.Parameters.AddWithValue("@URL", p.URL);

                if (p.SiteID == null) //Get new id number
                {
                    return cmd.ExecuteScalar().ToString().ConvertToNullableInt();
                }
                else //Return id if worked
                {
                    cmd.ExecuteScalar();

                    return p.SiteID;
                }
            }
        }

        public long? AddUser(int siteID, int userID)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = "";

                sql = @"INSERT INTO `siteuser` (`SiteUserID`, `SiteID`, `UserID`) VALUES (NULL, @SiteID, @UserID);
                            SELECT LAST_INSERT_ID();";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@SiteID", siteID);
                cmd.Parameters.AddWithValue("@UserID", userID);

                return cmd.ExecuteScalar().ToString().ConvertToNullableInt();
            }
        }

        public void RemoveUser(int siteID, int userID)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = "";

                sql = @"delete FROM `siteuser` where siteid = @SiteID AND userid = @UserID;";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@SiteID", siteID);
                cmd.Parameters.AddWithValue("@UserID", userID);

                cmd.ExecuteNonQuery();
            }
        }

        internal MDO.RESTDataEntities.Standard.Site ConvertMySQLToEntity(MySqlDataReader reader)
        {
            MDO.RESTDataEntities.Standard.Site p = new MDO.RESTDataEntities.Standard.Site();

            p.SiteID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "SiteID"));
            p.GUID = DBUtilities.ReturnSafeString(reader, "GUID");
            p.Name = DBUtilities.ReturnSafeString(reader, "Name");
            p.Active = DBUtilities.ReturnBoolean(reader, "Active");
            p.AllowAllUsers = DBUtilities.ReturnBoolean(reader, "AllowAllUsers");
            p.RegisterSuccessPath = DBUtilities.ReturnSafeString(reader, "RegisterSuccessPath");
            p.URL = DBUtilities.ReturnSafeString(reader, "URL");

            return p;
        }

        internal MDO.RESTDataEntities.Standard.SiteUser ConvertSiteUserMySQLToEntity(MySqlDataReader reader)
        {
            MDO.RESTDataEntities.Standard.SiteUser p = new MDO.RESTDataEntities.Standard.SiteUser();

            p.SiteUserID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "SiteUserID"));
            p.SiteID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "SiteID"));
            p.UserID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "UserID"));

            return p;
        }
    }
}
