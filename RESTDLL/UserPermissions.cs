using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace RESTDLL
{
    public class UserPermissions : DBManager
    {

        #region BoringStuff

        string ConnectionString { get; set; }

        MySqlConnection sqlConnection { get; set; }

        public UserPermissions(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public UserPermissions(MySqlConnection sqlConnection)
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

        public List<MDO.RESTDataEntities.Standard.UserPermission> GetAllUserPermissions(bool? active)
        {
            List<MDO.RESTDataEntities.Standard.UserPermission> p = new List<MDO.RESTDataEntities.Standard.UserPermission>();

            var s = GetConnection();

            var sql = "SELECT * FROM userpermission";

            if (active == true)
            {
                sql = "SELECT * FROM userpermission where active = 1";
            }
            else
            {
                sql = "SELECT * FROM userpermission where active = 0";
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

        public List<MDO.RESTDataEntities.Standard.UserPermission> GetAllUserPermissionsByUserID(int userID)
        {
            List<MDO.RESTDataEntities.Standard.UserPermission> p = new List<MDO.RESTDataEntities.Standard.UserPermission>();

            var s = GetConnection();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM userpermission where UserID = @ID;", conn);
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

        public MDO.RESTDataEntities.Standard.UserPermission GetUserPermissionByID(int id)
        {
            MDO.RESTDataEntities.Standard.UserPermission person = new MDO.RESTDataEntities.Standard.UserPermission();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM userpermission Where userpermissionID = @ID;", conn);

                cmd.Parameters.AddWithValue("@ID", id);
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

        public long? SaveUserPermission(MDO.RESTDataEntities.Standard.UserPermission p)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = "";

                if (p.UserPermissionID == null)
                {
                    sql = @"INSERT INTO `userpermission` (`UserPermissionID`, `UserID`, `PermissionID`, `ModifiedDate`, `Active`) VALUES (NULL, @UserID, @PermissionID, NULL, @Active);
                            SELECT LAST_INSERT_ID();";
                }
                else
                {
                    sql = @"UPDATE `userpermission` SET UserID = @UserID, PermissionID = @PermissionID, Active = @Active WHERE UserPermissionID = @ID;";
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", p.UserPermissionID);
                cmd.Parameters.AddWithValue("@UserID", p.UserID);
                cmd.Parameters.AddWithValue("@PermissionID", p.PermissionID);
                cmd.Parameters.AddWithValue("@Active", p.Active);

                if (p.UserPermissionID == null) //Get new id number
                {
                    return cmd.ExecuteScalar().ToString().ConvertToNullableInt();
                }
                else //Return id if worked
                {
                    cmd.ExecuteScalar();

                    return p.UserPermissionID;
                }
            }
        }

        public void RemoveUserPermission(int userID, int permissionID)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = "";

                sql = @"delete FROM `userpermission` where userID = @UserID AND permissionID = @PermissionID;";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@PermissionID", permissionID);
                cmd.Parameters.AddWithValue("@UserID", userID);

                cmd.ExecuteNonQuery();
            }
        }

        internal MDO.RESTDataEntities.Standard.UserPermission ConvertMySQLToEntity(MySqlDataReader reader)
        {
            MDO.RESTDataEntities.Standard.UserPermission p = new MDO.RESTDataEntities.Standard.UserPermission();

            p.UserPermissionID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "UserPermissionID"));
            p.UserID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "UserID"));
            p.PermissionID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "PermissionID"));
            p.ModifiedDate = DBUtilities.ReturnDateTime(reader, "ModifiedDate");
            p.Active = DBUtilities.ReturnBoolean(reader, "Active");

            return p;
        }
    }
}
