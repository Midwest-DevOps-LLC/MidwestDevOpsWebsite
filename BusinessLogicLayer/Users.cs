using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using MDO.Utility.Standard;

namespace BusinessLogicLayer
{
    public class Users : BLLManager, IDisposable
    {

        public Users(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public Users(MySqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public bool VerifyPassword(DataEntities.User user, string userPassword)
        {
            string s = TextHasher.Hash(userPassword, user.UUID);
            return TextHasher.Verify(userPassword, user.Password, user.UUID);
        }

        public string HashPassword(DataEntities.User user, string userPassword)
        {
            string s = TextHasher.Hash(userPassword, user.UUID);
            return s;
        }

        public List<DataEntities.User> GetAllUsers()
        {
            try
            {
                DatabaseLogicLayer.Users userDLL = new DatabaseLogicLayer.Users(GetConnection());

                return userDLL.GetAllUsers();
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public DataEntities.User GetUserByID(int userID)
        {
            try
            {
                DatabaseLogicLayer.Users userDLL = new DatabaseLogicLayer.Users(GetConnection());

                return userDLL.GetUserByID(userID);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public DataEntities.User GetUserByEmail(string email)
        {
            try
            {
                DatabaseLogicLayer.Users userDLL = new DatabaseLogicLayer.Users(GetConnection());

                return userDLL.GetUserByEmail(email);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public DataEntities.User GetUserByUsername(string userName)
        {
            try
            {
                DatabaseLogicLayer.Users userDLL = new DatabaseLogicLayer.Users(GetConnection());

                return userDLL.GetUserByUsername(userName.ToLower());
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public DataEntities.User GetUserByUUID(string UUID)
        {
            try
            {
                DatabaseLogicLayer.Users userDLL = new DatabaseLogicLayer.Users(GetConnection());

                return userDLL.GetUserByUUID(UUID);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public long? SaveUser(DataEntities.User user)
        {
            try
            {
                DatabaseLogicLayer.Users userDLL = new DatabaseLogicLayer.Users(GetConnection());

                return userDLL.SaveUser(user);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }
    }
}
