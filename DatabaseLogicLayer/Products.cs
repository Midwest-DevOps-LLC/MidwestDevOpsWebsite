using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DatabaseLogicLayer
{
    public class Products : DBManager
    {

        #region BoringStuff

        string ConnectionString { get; set; }

        MySqlConnection sqlConnection { get; set; }

        public Products(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public Products(MySqlConnection sqlConnection)
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

        public List<DataEntities.Product> GetAllProducts()
        {
            List<DataEntities.Product> p = new List<DataEntities.Product>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM product;", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        p.Add(ConvertMySQLToEntity(reader));
                    }
                }

                foreach (var product in p)
                {
                    product.productPricings = GetProductPricings(product.ProductID.Value);
                    product.productPictures = GetProductPictures(product.ProductID.Value);
                    product.productChangelogs = GetProductChangeLogs(product.ProductID.Value);
                }
            }

            return p;
        }

        public DataEntities.Product GetProductByID(int productID)
        {
            DataEntities.Product person = new DataEntities.Product();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM product Where ProductID = @ID;", conn);

                cmd.Parameters.AddWithValue("@ID", productID);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        person = ConvertMySQLToEntity(reader);
                    }
                }

                person.productPricings = GetProductPricings(person.ProductID.Value);
                person.productPictures = GetProductPictures(person.ProductID.Value);
                person.productChangelogs = GetProductChangeLogs(person.ProductID.Value);
            }

            return person;
        }

        public DataEntities.Product GetProductByName(string name)
        {
            DataEntities.Product person = new DataEntities.Product();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM product Where ProductName = @Name;", conn);

                cmd.Parameters.AddWithValue("@Name", name);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        person = ConvertMySQLToEntity(reader);
                    }
                }

                person.productPricings = GetProductPricings(person.ProductID.Value);
                person.productPictures = GetProductPictures(person.ProductID.Value);
                person.productChangelogs = GetProductChangeLogs(person.ProductID.Value);
            }

            return person;
        }

        public long? SaveProduct(DataEntities.Product p)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = "";

                if (p.ProductID == null)
                {
                    sql = @"INSERT INTO `product` (`ProductID`, `ProductName`, `Description`, `ShortDescription`, `ProductURL`, `ProductPicture`, `CreatedBy`, `CreatedDate`, `ModifiedBy`, `ModifedDate`, `Active`) VALUES (NULL, @ProductName, @Description, @ShortDescription, @ProductURL, @ProductPicture, @CreatedBy, CURRENT_TIMESTAMP, NULL, CURRENT_TIMESTAMP, @Active);
                            SELECT LAST_INSERT_ID();";
                }
                else
                {
                    sql = @"UPDATE `product` SET ProductID = @ProductID, ProductName = @ProductName, Description = @Description, ShortDescription = @ShortDescription, ProductURL = @ProductURL, ProductPicture = @ProductPicture, CreatedBy = @CreatedBy, CreatedDate = @CreatedDate, ModifiedBy = @ModifiedBy, ModifiedDate = @ModifiedDate, Active = @Active WHERE ProductID = @ID;";
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", p.ProductID);
                cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                cmd.Parameters.AddWithValue("@Description", p.Description);
                cmd.Parameters.AddWithValue("@ShortDescription", p.ShortDescription);
                cmd.Parameters.AddWithValue("@ProductURL", p.ProductURL);
                cmd.Parameters.AddWithValue("@ProductPicture", p.ProductMainPicture);
                cmd.Parameters.AddWithValue("@CreatedBy", p.CreatedBy);
                cmd.Parameters.AddWithValue("@CreatedDate", p.CreatedDate);
                cmd.Parameters.AddWithValue("@ModifiedBy", p.ModifiedBy);
                cmd.Parameters.AddWithValue("@ModifiedDate", p.ModifiedDate);
                cmd.Parameters.AddWithValue("@Active", p.Active);


                if (p.ProductID == null) //Get new id number
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

        internal DataEntities.Product ConvertMySQLToEntity(MySqlDataReader reader)
        {
            DataEntities.Product p = new DataEntities.Product();

            p.ProductID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "ProductID"));
            p.ProductName = DBUtilities.ReturnSafeString(reader, "ProductName");
            p.Description = DBUtilities.ReturnSafeString(reader, "Description");
            p.ShortDescription = DBUtilities.ReturnSafeString(reader, "ShortDescription");
            p.ProductURL = DBUtilities.ReturnSafeString(reader, "ProductURL");
            p.ProductMainPicture = DBUtilities.ReturnSafeString(reader, "ProductPicture");
            p.Active = DBUtilities.ReturnBoolean(reader, "Active");
            p.CreatedBy = DBUtilities.ReturnSafeInt(reader, "CreatedBy");
            p.CreatedDate = DBUtilities.ReturnSafeDateTime(reader, "CreatedDate");
            p.ModifiedBy = DBUtilities.ReturnSafeInt(reader, "ModifiedBy");
            p.ModifiedDate = DBUtilities.ReturnSafeDateTime(reader, "ModifiedDate");

            return p;
        }

        /////////////////////
        // Product Pricing //
        /////////////////////

        internal List<DataEntities.Product.ProductPricing> GetProductPricings(int productID)
        {
            var person = new List<DataEntities.Product.ProductPricing>();

            using (MySqlConnection conn = GetConnection())
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM productpricing Where ProductID = @ID;", conn);

                cmd.Parameters.AddWithValue("@ID", productID);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        person.Add(ConvertMySQLToEntityProductPricing(reader));
                    }

                    return person;
                }
            }
        }

        internal DataEntities.Product.ProductPricing ConvertMySQLToEntityProductPricing(MySqlDataReader reader)
        {
            DataEntities.Product.ProductPricing p = new DataEntities.Product.ProductPricing();

            p.ProductPricingID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "ProductPricingID"));
            p.ProductID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "ProductID"));
            p.Name = DBUtilities.ReturnSafeString(reader, "Name");
            p.Amount = DBUtilities.ReturnSafeDouble(reader, "Amount");
            p.isMonthly = DBUtilities.ReturnBoolean(reader, "isMonthly");
            p.SubText = DBUtilities.ReturnSafeString(reader, "SubText");
            p.isPrimary = DBUtilities.ReturnBoolean(reader, "isPrimary");

            return p;
        }

        /////////////////////
        // Product Picture //
        /////////////////////

        internal List<DataEntities.Product.ProductPicture> GetProductPictures(int productID)
        {
            var person = new List<DataEntities.Product.ProductPicture>();

            using (MySqlConnection conn = GetConnection())
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM productpicture Where ProductID = @ID;", conn);

                cmd.Parameters.AddWithValue("@ID", productID);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        person.Add(ConvertMySQLToEntityProductPicture(reader));
                    }

                    return person;
                }
            }
        }

        internal DataEntities.Product.ProductPicture ConvertMySQLToEntityProductPicture(MySqlDataReader reader)
        {
            DataEntities.Product.ProductPicture p = new DataEntities.Product.ProductPicture();

            p.ProductPictureID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "ProductPictureID"));
            p.ProductID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "ProductID"));
            p.Path = DBUtilities.ReturnSafeString(reader, "Path");
            p.Alt = DBUtilities.ReturnSafeString(reader, "Alt");

            return p;
        }

        ///////////////////////
        // Product Changelog //
        ///////////////////////

        internal List<DataEntities.Product.ProductChangelog> GetProductChangeLogs(int productID)
        {
            var person = new List<DataEntities.Product.ProductChangelog>();

            using (MySqlConnection conn = GetConnection())
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM productchangelog Where ProductID = @ID;", conn);

                cmd.Parameters.AddWithValue("@ID", productID);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        person.Add(ConvertMySQLToEntityProductChangeLog(reader));
                    }

                    return person;
                }
            }
        }

        internal DataEntities.Product.ProductChangelog ConvertMySQLToEntityProductChangeLog(MySqlDataReader reader)
        {
            DataEntities.Product.ProductChangelog p = new DataEntities.Product.ProductChangelog();

            p.ProductChangeLogID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "ProductChangeLogID"));
            p.ProductID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "ProductID"));
            p.Version = DBUtilities.ReturnSafeString(reader, "Version");
            p.HTML = DBUtilities.ReturnSafeString(reader, "HTML");

            return p;
        }
    }
}
