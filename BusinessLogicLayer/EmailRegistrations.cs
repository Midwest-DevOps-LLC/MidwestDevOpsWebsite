using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace BusinessLogicLayer
{
    public class EmailRegistrations : BLLManager, IDisposable
    {

        public EmailRegistrations(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public EmailRegistrations(MySqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public int SetAllEmailRegistractionsToInactive(int userID)
        {
            try
            {
                DatabaseLogicLayer.EmailRegistrations userDLL = new DatabaseLogicLayer.EmailRegistrations(GetConnection());

                return userDLL.SetAllEmailRegistractionsToInactive(userID);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return 0;
        }

        public List<DataEntities.EmailRegistration> GetAllEmailRegistrations()
        {
            try
            {
                DatabaseLogicLayer.EmailRegistrations userDLL = new DatabaseLogicLayer.EmailRegistrations(GetConnection());

                return userDLL.GetAllEmailRegistrations();
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public DataEntities.EmailRegistration GetEmailRegistrationByID(int registrationID)
        {
            try
            {
                DatabaseLogicLayer.EmailRegistrations userDLL = new DatabaseLogicLayer.EmailRegistrations(GetConnection());

                return userDLL.GetEmailRegistrationByID(registrationID);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public DataEntities.EmailRegistration GetEmailRegistrationByUUID(string UUID)
        {
            try
            {
                DatabaseLogicLayer.EmailRegistrations userDLL = new DatabaseLogicLayer.EmailRegistrations(GetConnection());

                return userDLL.GetEmailRegistrationByUUID(UUID);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public DataEntities.EmailRegistration GetEmailRegistrationByUserID(int userID)
        {
            try
            {
                DatabaseLogicLayer.EmailRegistrations userDLL = new DatabaseLogicLayer.EmailRegistrations(GetConnection());

                return userDLL.GetEmailRegistrationByUserID(userID);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }

        public long? SaveEmailRegistration(DataEntities.EmailRegistration emailRegistration)
        {
            try
            {
                DatabaseLogicLayer.EmailRegistrations userDLL = new DatabaseLogicLayer.EmailRegistrations(GetConnection());

                return userDLL.SaveEmailRegistration(emailRegistration);
            }
            catch (Exception e)
            {
                Logging.SaveLog(new Log() { time = DateTime.UtcNow, exception = e });
            }

            return null;
        }
    }
}
