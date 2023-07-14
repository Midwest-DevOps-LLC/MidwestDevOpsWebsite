using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using MDO.Utility.Standard;

namespace BusinessLogicLayer
{
    public class Products : BLLManager, IDisposable
    {

        public Products(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public Products(MySqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public List<DataEntities.Product> GetAllProducts()
        {
            try
            {
                DatabaseLogicLayer.Products userDLL = new DatabaseLogicLayer.Products(GetConnection());

                return userDLL.GetAllProducts();
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public DataEntities.Product GetProductByID(int productID)
        {
            try
            {
                DatabaseLogicLayer.Products userDLL = new DatabaseLogicLayer.Products(GetConnection());

                return userDLL.GetProductByID(productID);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public DataEntities.Product GetProductByName(string productName)
        {
            try
            {
                DatabaseLogicLayer.Products userDLL = new DatabaseLogicLayer.Products(GetConnection());

                return userDLL.GetProductByName(productName);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public long? SaveProduct(DataEntities.Product product)
        {
            try
            {
                DatabaseLogicLayer.Products userDLL = new DatabaseLogicLayer.Products(GetConnection());

                return userDLL.SaveProduct(product);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }
    }
}
