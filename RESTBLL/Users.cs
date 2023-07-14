using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using MDO.Utility.Standard;

namespace RESTBLL
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

        public bool VerifyPassword(MDO.RESTDataEntities.Standard.User user, string userPassword)
        {
            string s = TextHasher.Hash(userPassword, user.UUID);
            return TextHasher.Verify(userPassword, user.Password, user.UUID);
        }

        public string HashPassword(MDO.RESTDataEntities.Standard.User user, string userPassword)
        {
            string s = TextHasher.Hash(userPassword, user.UUID);
            return s;
        }

        public List<MDO.RESTDataEntities.Standard.User> GetAllUsers()
        {
            try
            {
                RESTDLL.Users userDLL = new RESTDLL.Users(GetConnection());

                return userDLL.GetAllUsers();
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public MDO.RESTDataEntities.Standard.User GetUserByID(int userID)
        {
            try
            {
                RESTDLL.Users userDLL = new RESTDLL.Users(GetConnection());

                return userDLL.GetUserByID(userID);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public MDO.RESTDataEntities.Standard.User GetUserByEmail(string email)
        {
            try
            {
                RESTDLL.Users userDLL = new RESTDLL.Users(GetConnection());

                return userDLL.GetUserByEmail(email);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public MDO.RESTDataEntities.Standard.User GetUserByUsername(string userName)
        {
            try
            {
                RESTDLL.Users userDLL = new RESTDLL.Users(GetConnection());

                return userDLL.GetUserByUsername(userName.ToLower());
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public MDO.RESTDataEntities.Standard.User GetUserByUUID(string UUID)
        {
            try
            {
                RESTDLL.Users userDLL = new RESTDLL.Users(GetConnection());

                return userDLL.GetUserByUUID(UUID);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public long? SaveUser(MDO.RESTDataEntities.Standard.User user)
        {
            try
            {
                RESTDLL.Users userDLL = new RESTDLL.Users(GetConnection());

                return userDLL.SaveUser(user);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }
    }
}
