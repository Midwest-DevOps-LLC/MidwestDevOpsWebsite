using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using MDO.Utility.Standard;

namespace RESTBLL
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

        public List<MDO.RESTDataEntities.Standard.ThirdPartyUser> GetAllThirdPartyUsers()
        {
            try
            {
                RESTDLL.ThirdPartyUser userDLL = new RESTDLL.ThirdPartyUser(GetConnection());

                return userDLL.GetAllThirdPartyUsers();
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public List<MDO.RESTDataEntities.Standard.ThirdPartyUser> GetAllThirdPartyUsersByUserID(int userID)
        {
            try
            {
                RESTDLL.ThirdPartyUser userDLL = new RESTDLL.ThirdPartyUser(GetConnection());

                return userDLL.GetAllThirdPartyUsersByUserID(userID);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public MDO.RESTDataEntities.Standard.ThirdPartyUser GetThirdPartyByName(int thirdPartyUserID)
        {
            try
            {
                RESTDLL.ThirdPartyUser userDLL = new RESTDLL.ThirdPartyUser(GetConnection());

                return userDLL.GetThirdPartyUserByID(thirdPartyUserID);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public long? SaveThirdPartyUser(MDO.RESTDataEntities.Standard.ThirdPartyUser thirdParty)
        {
            try
            {
                RESTDLL.ThirdPartyUser userDLL = new RESTDLL.ThirdPartyUser(GetConnection());

                return userDLL.SaveThirdPartyUser(thirdParty);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }
    }
}
