using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace RESTDLL
{
    public class ExternalApplication : DBManager
    {

        #region BoringStuff

        string ConnectionString { get; set; }

        MySqlConnection sqlConnection { get; set; }

        public ExternalApplication(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public ExternalApplication(MySqlConnection sqlConnection)
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

        public List<MDO.RESTDataEntities.Standard.ExternalApplication> GetAllExternalApplications()
        {
            List<MDO.RESTDataEntities.Standard.ExternalApplication> p = new List<MDO.RESTDataEntities.Standard.ExternalApplication>();

            var s = GetConnection();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM externalapplication", conn);
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

        public MDO.RESTDataEntities.Standard.ExternalApplication GetExternalApplicationByID(int id)
        {
            MDO.RESTDataEntities.Standard.ExternalApplication person = new MDO.RESTDataEntities.Standard.ExternalApplication();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM externalapplication Where externalapplicationID = @ID;", conn);

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

        public long? SaveExternalApplication(MDO.RESTDataEntities.Standard.ExternalApplication p)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = "";

                if (p.ID == null)
                {
                    sql = @"INSERT INTO `externalapplication` (`ExternalApplicationID`, `Name`, `Description`, `CreatedDate`) VALUES (NULL, @Name, @Description, NULL);
                            SELECT LAST_INSERT_ID();";
                }
                else
                {
                    sql = @"UPDATE `externalapplication` SET Name = @Name, Description = @Description WHERE externalapplicationID = @ID;";
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", p.ID);
                cmd.Parameters.AddWithValue("@Name", p.Name);
                cmd.Parameters.AddWithValue("@Description", p.Description);

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

        internal MDO.RESTDataEntities.Standard.ExternalApplication ConvertMySQLToEntity(MySqlDataReader reader)
        {
            MDO.RESTDataEntities.Standard.ExternalApplication p = new MDO.RESTDataEntities.Standard.ExternalApplication();

            p.ID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "ExternalApplicationID"));
            p.Name = DBUtilities.ReturnSafeString(reader, "Name");
            p.Description = DBUtilities.ReturnSafeString(reader, "Description");
            p.CreatedDate = DBUtilities.ReturnDateTime(reader, "CreatedDate");

            return p;
        }

        #region LoadPermissions

        public List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission> GetAllExternalApplicationPermissions()
        {
            List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission> p = new List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission>();

            var s = GetConnection();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM externalapplicationpermission", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        p.Add(ConvertPermissionMySQLToEntity(reader));
                    }
                }
            }

            return p;
        }

        public List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission> GetExternalApplicationPermissionsByExternalApplicationID(int appID)
        {
            List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission> p = new List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission>();

            var s = GetConnection();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM externalapplicationpermission WHERE	ExternalApplicationID = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", appID);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        p.Add(ConvertPermissionMySQLToEntity(reader));
                    }
                }
            }

            return p;
        }

        public List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission> GetExternalApplicationPermissionsByExternalApplicationIDAndPermissionID(int appID, int permID)
        {
            List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission> p = new List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission>();

            var s = GetConnection();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM externalapplicationpermission WHERE	ExternalApplicationID = @ID AND PermissionID = @PermID", conn);
                cmd.Parameters.AddWithValue("@ID", appID);
                cmd.Parameters.AddWithValue("@PermID", permID);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        p.Add(ConvertPermissionMySQLToEntity(reader));
                    }
                }
            }

            return p;
        }

        public MDO.RESTDataEntities.Standard.ExternalApplication.Permission GetExternalApplicationPermissionByID(int id)
        {
            MDO.RESTDataEntities.Standard.ExternalApplication.Permission person = new MDO.RESTDataEntities.Standard.ExternalApplication.Permission();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM externalapplicationpermission Where ExternalApplicationPermissionID = @ID;", conn);

                cmd.Parameters.AddWithValue("@ID", id);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return ConvertPermissionMySQLToEntity(reader);
                    }

                    return null;
                }
            }
        }

        public long? SaveExternalApplicationPermission(MDO.RESTDataEntities.Standard.ExternalApplication.Permission p)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = "";

                if (p.ExternalApplicationPermissionID == null)
                {
                    sql = @"INSERT INTO `externalapplicationpermission` (`ExternalApplicationPermissionID`, `ExternalApplicationID`, `PermissionID`, `Required`, `Active`) VALUES (NULL, @ExternalApplicationID, @PermissionID, @Required, @Active);
                            SELECT LAST_INSERT_ID();";
                }
                else
                {
                    sql = @"UPDATE `externalapplicationpermission` SET ExternalApplicationID = @ExternalApplicationID, PermissionID = @PermissionID, Required = @Required, Active = @Active  WHERE ExternalApplicationPermissionID = @ID;";
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", p.ExternalApplicationPermissionID);
                cmd.Parameters.AddWithValue("@ExternalApplicationID", p.ExternalApplicationID);
                cmd.Parameters.AddWithValue("@PermissionID", p.PermissionID);
                cmd.Parameters.AddWithValue("@Required", p.IsRequired);
                cmd.Parameters.AddWithValue("@Active", p.Active);

                if (p.ExternalApplicationPermissionID == null) //Get new id number
                {
                    return cmd.ExecuteScalar().ToString().ConvertToNullableInt();
                }
                else //Return id if worked
                {
                    cmd.ExecuteScalar();

                    return p.ExternalApplicationPermissionID;
                }
            }
        }

        internal MDO.RESTDataEntities.Standard.ExternalApplication.Permission ConvertPermissionMySQLToEntity(MySqlDataReader reader)
        {
            MDO.RESTDataEntities.Standard.ExternalApplication.Permission p = new MDO.RESTDataEntities.Standard.ExternalApplication.Permission();

            p.ExternalApplicationPermissionID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "ExternalApplicationPermissionID"));
            p.ExternalApplicationID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "ExternalApplicationID"));
            p.PermissionID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "PermissionID"));
            p.IsRequired = DBUtilities.ReturnBoolean(reader, "Required");
            p.Active = DBUtilities.ReturnBoolean(reader, "Active");

            return p;
        }

        #endregion
    }
}
