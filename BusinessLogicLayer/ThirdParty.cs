using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using MDO.Utility.Standard;

namespace BusinessLogicLayer
{
    public class ThirdParty : BLLManager, IDisposable
    {

        public ThirdParty(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public ThirdParty(MySqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public List<DataEntities.ThirdParty> GetAllThirdParties()
        {
            try
            {
                DatabaseLogicLayer.ThirdParty userDLL = new DatabaseLogicLayer.ThirdParty(GetConnection());

                return userDLL.GetAllThirdParties();
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public DataEntities.ThirdParty GetThirdPartyByID(int thirdPartyID)
        {
            try
            {
                DatabaseLogicLayer.ThirdParty userDLL = new DatabaseLogicLayer.ThirdParty(GetConnection());

                return userDLL.GetThirdPartyByID(thirdPartyID);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public DataEntities.ThirdParty GetThirdPartyByName(string name)
        {
            try
            {
                DatabaseLogicLayer.ThirdParty userDLL = new DatabaseLogicLayer.ThirdParty(GetConnection());

                return userDLL.GetThirdPartyByName(name);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public long? SaveThirdParty(DataEntities.ThirdParty thirdParty)
        {
            try
            {
                DatabaseLogicLayer.ThirdParty userDLL = new DatabaseLogicLayer.ThirdParty(GetConnection());

                return userDLL.SaveThirdParty(thirdParty);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }
    }
}
