using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace RESTDLL
{
    public class EmailRegistrations : DBManager
    {

        #region BoringStuff

        string ConnectionString { get; set; }

        MySqlConnection sqlConnection { get; set; }

        public EmailRegistrations(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public EmailRegistrations(MySqlConnection sqlConnection)
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

        public List<MDO.RESTDataEntities.Standard.EmailRegistration> GetAllEmailRegistrations()
        {
            List<MDO.RESTDataEntities.Standard.EmailRegistration> p = new List<MDO.RESTDataEntities.Standard.EmailRegistration>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM emailregistration", conn);
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

        public MDO.RESTDataEntities.Standard.EmailRegistration GetEmailRegistrationByID(int emailRegistrationID)
        {
            MDO.RESTDataEntities.Standard.EmailRegistration person = new MDO.RESTDataEntities.Standard.EmailRegistration();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM emailregistration Where EmailRegistrationID = @ID;", conn);

                cmd.Parameters.AddWithValue("@ID", emailRegistrationID);
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

        public MDO.RESTDataEntities.Standard.EmailRegistration GetEmailRegistrationByUUID(string UUID)
        {
            MDO.RESTDataEntities.Standard.EmailRegistration person = new MDO.RESTDataEntities.Standard.EmailRegistration();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM emailregistration Where UUID = @ID;", conn);

                cmd.Parameters.AddWithValue("@ID", UUID);
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

        public MDO.RESTDataEntities.Standard.EmailRegistration GetEmailRegistrationByUserID(int userID)
        {
            MDO.RESTDataEntities.Standard.EmailRegistration person = new MDO.RESTDataEntities.Standard.EmailRegistration();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM emailregistration Where UserID = @UserID;", conn);

                cmd.Parameters.AddWithValue("@UserID", userID);
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

        public int SetAllEmailRegistractionsToInactive(int userID)
        {
            MDO.RESTDataEntities.Standard.EmailRegistration person = new MDO.RESTDataEntities.Standard.EmailRegistration();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE emailregistration SET Active = 0 Where UserID= @UserID;", conn);

                cmd.Parameters.AddWithValue("@UserID", userID);

                return cmd.ExecuteNonQuery();
            }
        }

        public long? SaveEmailRegistration(MDO.RESTDataEntities.Standard.EmailRegistration p)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = "";

                if (p.EmailRegistrationID == null)
                {
                    sql = @"INSERT INTO `emailregistration` (`EmailRegistrationID`, `UUID`, `UserID`, `Active`, `Application`) VALUES (NULL, @UUID, @UserID, @Active, @Application);
                            SELECT LAST_INSERT_ID();";
                }
                else
                {
                    sql = @"UPDATE `emailregistration` SET UUID = @UUID, UserID = @UserID, Active = @Active, Application = @Application WHERE EmailRegistrationID = @ID;";
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", p.UserID);
                cmd.Parameters.AddWithValue("@UserID", p.UserID);
                cmd.Parameters.AddWithValue("@UUID", p.UUID);
                cmd.Parameters.AddWithValue("@Active", p.Active);
                cmd.Parameters.AddWithValue("@Application", p.Application);


                if (p.EmailRegistrationID == null) //Get new id number
                {
                    return cmd.ExecuteScalar().ToString().ConvertToNullableInt();
                }
                else //Return id if worked
                {
                    cmd.ExecuteScalar();

                    return p.EmailRegistrationID;
                }
            }
        }

        internal MDO.RESTDataEntities.Standard.EmailRegistration ConvertMySQLToEntity(MySqlDataReader reader)
        {
            MDO.RESTDataEntities.Standard.EmailRegistration p = new MDO.RESTDataEntities.Standard.EmailRegistration();

            p.EmailRegistrationID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "EmailRegistrationID"));
            p.UUID = DBUtilities.ReturnSafeString(reader, "UUID");
            p.UserID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "UserID"));
            p.Active = DBUtilities.ReturnBoolean(reader, "Active");
            p.Application = DBUtilities.ReturnSafeString(reader, "Application");

            return p;
        }
    }
}
