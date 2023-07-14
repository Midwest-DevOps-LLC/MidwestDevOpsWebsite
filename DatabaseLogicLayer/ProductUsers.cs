using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DatabaseLogicLayer
{
    public class ProductUsers : DBManager
    {

        #region BoringStuff

        string ConnectionString { get; set; }

        MySqlConnection sqlConnection { get; set; }

        public ProductUsers(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public ProductUsers(MySqlConnection sqlConnection)
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

        public List<DataEntities.ProductUser> GetAllProductUsers()
        {
            List<DataEntities.ProductUser> p = new List<DataEntities.ProductUser>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `productuser` as pu join product as p on pu.ProductID = p.ProductID join `user` as u on pu.UserID = u.UserID", conn);
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

        public DataEntities.ProductUser GetProductUserByID(int projectUserID)
        {
            DataEntities.ProductUser person = new DataEntities.ProductUser();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `productuser` as pu join product as p on pu.ProductID = p.ProductID join `user` as u on pu.UserID = u.UserID Where ProductUserID = @ID;", conn);

                cmd.Parameters.AddWithValue("@ID", projectUserID);
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

        public List<DataEntities.ProductUser> GetProductUserByUserID(int userID)
        {
            List<DataEntities.ProductUser> p = new List<DataEntities.ProductUser>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `productuser` as pu join product as p on pu.ProductID = p.ProductID join `user` as u on pu.UserID = u.UserID Where pu.UserID = @UserID;", conn);

                cmd.Parameters.AddWithValue("@UserID", userID);
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

        public long? SaveProductUser(DataEntities.ProductUser p)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = "";

                if (p.ProductUserID == null)
                {
                    sql = @"INSERT INTO `productuser` (`ProductUserID`, `ProductID`, `UserID`, `ProductKey`, `CreatedBy`, `CreatedDate`, `ModifiedBy`, `ModifiedDate`, `Active`) VALUES (NULL, @ProductID, @UserID, @ProductKey, @CreatedBy, CURRENT_TIMESTAMP, NULL, CURRENT_TIMESTAMP, @Active);
                            SELECT LAST_INSERT_ID();";
                }
                else
                {
                    sql = @"UPDATE `emailregistration` SET ProductID = @ProductID, UserID = @UserID, ProductKey = @ProductKey, CreatedBy = @CreatedBy, CreatedDate = @CreatedDate, ModifiedBy = @ModifiedBy, ModifiedDate = @ModifiedDate, Active = @Active WHERE ProductUserID = @ID;";
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", p.ProductUserID);
                cmd.Parameters.AddWithValue("@ProductID", p.ProductID);
                cmd.Parameters.AddWithValue("@UserID", p.UserID);
                cmd.Parameters.AddWithValue("@ProductKey", p.Key);
                cmd.Parameters.AddWithValue("@CreatedBy", p.CreatedBy);
                cmd.Parameters.AddWithValue("@CreatedDate", p.CreatedDate);
                cmd.Parameters.AddWithValue("@ModifiedBy", p.ModifiedBy);
                cmd.Parameters.AddWithValue("@ModifiedDate", p.ModifiedDate);
                cmd.Parameters.AddWithValue("@Active", p.Active);


                if (p.ProductUserID == null) //Get new id number
                {
                    return cmd.ExecuteScalar().ToString().ConvertToNullableInt();
                }
                else //Return id if worked
                {
                    cmd.ExecuteScalar();

                    return p.ProductID;
                }
            }
        }

        internal DataEntities.ProductUser ConvertMySQLToEntity(MySqlDataReader reader)
        {
            DataEntities.ProductUser p = new DataEntities.ProductUser();

            p.ProductID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "ProductID"));
            p.ProductUserID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "ProductUserID"));
            p.UserID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "UserID"));
            p.ProductName = DBUtilities.ReturnSafeString(reader, "ProductName");
            p.ProductDescription = DBUtilities.ReturnSafeString(reader, "Description");
            p.ProductUrl = DBUtilities.ReturnSafeString(reader, "ProductUrl");
            p.ProductPicture = DBUtilities.ReturnSafeString(reader, "ProductPicture");
            p.Key = DBUtilities.ReturnSafeString(reader, "ProductKey");
            p.Active = DBUtilities.ReturnBoolean(reader, "Active");
            p.CreatedBy = DBUtilities.ReturnSafeInt(reader, "CreatedBy");
            p.CreatedDate = DBUtilities.ReturnSafeDateTime(reader, "CreatedDate");
            p.ModifiedBy = DBUtilities.ReturnSafeInt(reader, "ModifiedBy");
            p.ModifiedDate = DBUtilities.ReturnSafeDateTime(reader, "ModifiedDate");

            return p;
        }
    }
}
