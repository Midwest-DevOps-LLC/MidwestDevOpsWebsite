using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using MDO.Utility.Standard;

namespace BusinessLogicLayer
{
    public class ThirdPartyUser : BLLManager, IDisposable
    {

        public ThirdPartyUser(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public ThirdPartyUser(MySqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public List<DataEntities.ThirdPartyUser> GetAllThirdPartyUsers()
        {
            try
            {
                DatabaseLogicLayer.ThirdPartyUser userDLL = new DatabaseLogicLayer.ThirdPartyUser(GetConnection());

                return userDLL.GetAllThirdPartyUsers();
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public List<DataEntities.ThirdPartyUser> GetAllThirdPartyUsersByUserID(int userID)
        {
            try
            {
                DatabaseLogicLayer.ThirdPartyUser userDLL = new DatabaseLogicLayer.ThirdPartyUser(GetConnection());

                return userDLL.GetAllThirdPartyUsersByUserID(userID);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public DataEntities.ThirdPartyUser GetThirdPartyByName(int thirdPartyUserID)
        {
            try
            {
                DatabaseLogicLayer.ThirdPartyUser userDLL = new DatabaseLogicLayer.ThirdPartyUser(GetConnection());

                return userDLL.GetThirdPartyUserByID(thirdPartyUserID);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public long? SaveThirdPartyUser(DataEntities.ThirdPartyUser thirdParty)
        {
            try
            {
                DatabaseLogicLayer.ThirdPartyUser userDLL = new DatabaseLogicLayer.ThirdPartyUser(GetConnection());

                return userDLL.SaveThirdPartyUser(thirdParty);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }
    }
}
