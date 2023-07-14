using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace BusinessLogicLayer
{
    public class ProductUsers : BLLManager, IDisposable
    {

        public ProductUsers(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public ProductUsers(MySqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public List<DataEntities.ProductUser> GetAllProductUsers()
        {
            try
            {
                DatabaseLogicLayer.ProductUsers userDLL = new DatabaseLogicLayer.ProductUsers(GetConnection());

                return userDLL.GetAllProductUsers();
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public DataEntities.ProductUser GetProductUserByID(int productUserID)
        {
            try
            {
                DatabaseLogicLayer.ProductUsers userDLL = new DatabaseLogicLayer.ProductUsers(GetConnection());

                return userDLL.GetProductUserByID(productUserID);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public List<DataEntities.ProductUser> GetProductUserByUserID(int userID)
        {
            try
            {
                DatabaseLogicLayer.ProductUsers userDLL = new DatabaseLogicLayer.ProductUsers(GetConnection());

                return userDLL.GetProductUserByUserID(userID);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public long? SaveProductUser(DataEntities.ProductUser productUser)
        {
            try
            {
                DatabaseLogicLayer.ProductUsers userDLL = new DatabaseLogicLayer.ProductUsers(GetConnection());

                return userDLL.SaveProductUser(productUser);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }
    }
}
