using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace RESTDLL
{
    public class Permissions : DBManager
    {

        #region BoringStuff

        string ConnectionString { get; set; }

        MySqlConnection sqlConnection { get; set; }

        public Permissions(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public Permissions(MySqlConnection sqlConnection)
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

        public List<MDO.RESTDataEntities.Standard.Permission> GetAllPermissions()
        {
            List<MDO.RESTDataEntities.Standard.Permission> p = new List<MDO.RESTDataEntities.Standard.Permission>();

            var s = GetConnection();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM permission", conn);
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

        public List<MDO.RESTDataEntities.Standard.Permission> GetAllPermissionsByParentPermissionID(int parentID)
        {
            List<MDO.RESTDataEntities.Standard.Permission> p = new List<MDO.RESTDataEntities.Standard.Permission>();

            var s = GetConnection();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM permission where ParentPermissionID = @ID", conn);

                cmd.Parameters.AddWithValue("@ID", parentID);

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

        public MDO.RESTDataEntities.Standard.Permission GetPermissionByID(int thirdPartyUserID)
        {
            MDO.RESTDataEntities.Standard.Permission person = new MDO.RESTDataEntities.Standard.Permission();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM permission Where permissionid = @ID;", conn);

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

        public long? SavePermission(MDO.RESTDataEntities.Standard.Permission p)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = "";

                if (p.ID == null)
                {
                    sql = @"INSERT INTO `permission` (`PermissionID`, `ParentPermissionID`, `Name`, `Description`, `CreatedDate`, `Ordinal`, `UserAlwaysHas`) VALUES (NULL, @ParentPermissionID, @Name, @Description, NULL, @Ordinal, @UserAlwaysHas);
                            SELECT LAST_INSERT_ID();";
                }
                else
                {
                    sql = @"UPDATE `permission` SET ParentPermissionID = @ParentPermissionID, Name = @Name, Description = @Description, Ordinal = @Ordinal, UserAlwaysHas = @UserAlwaysHas WHERE PermissionID = @ID;";
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", p.ID);
                cmd.Parameters.AddWithValue("@ParentPermissionID", p.ParentPermissionID);
                cmd.Parameters.AddWithValue("@Name", p.Name);
                cmd.Parameters.AddWithValue("@Description", p.Description);
                cmd.Parameters.AddWithValue("@Ordinal", p.Ordinal);
                cmd.Parameters.AddWithValue("@UserAlwaysHas", p.UserAlwaysHas);

                if (p.ID == null) //Get new id number
                {
                    return cmd.ExecuteScalar().ToString().ConvertToNullableInt();
                }
                else //Return id if worked
                {
                    cmd.ExecuteScalar();

                    return p.ID;
                }
            }
        }

        internal MDO.RESTDataEntities.Standard.Permission ConvertMySQLToEntity(MySqlDataReader reader)
        {
            MDO.RESTDataEntities.Standard.Permission p = new MDO.RESTDataEntities.Standard.Permission();

            p.ID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "PermissionID"));
            p.ParentPermissionID = DBUtilities.ReturnSafeInt(reader, "ParentPermissionID");
            p.Name = DBUtilities.ReturnSafeString(reader, "Name");
            p.Description = DBUtilities.ReturnSafeString(reader, "Description");
            p.CreatedDate = DBUtilities.ReturnDateTime(reader, "CreatedDate");
            p.Ordinal = DBUtilities.ReturnSafeInt(reader, "Ordinal").GetValueOrDefault();
            p.UserAlwaysHas = DBUtilities.ReturnBoolean(reader, "UserAlwaysHas");

            return p;
        }
    }
}
