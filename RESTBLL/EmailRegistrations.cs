using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace RESTBLL
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
                RESTDLL.EmailRegistrations userDLL = new RESTDLL.EmailRegistrations(GetConnection());

                return userDLL.SetAllEmailRegistractionsToInactive(userID);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return 0;
        }

        public List<MDO.RESTDataEntities.Standard.EmailRegistration> GetAllEmailRegistrations()
        {
            try
            {
                RESTDLL.EmailRegistrations userDLL = new RESTDLL.EmailRegistrations(GetConnection());

                return userDLL.GetAllEmailRegistrations();
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public MDO.RESTDataEntities.Standard.EmailRegistration GetEmailRegistrationByID(int registrationID)
        {
            try
            {
                RESTDLL.EmailRegistrations userDLL = new RESTDLL.EmailRegistrations(GetConnection());

                return userDLL.GetEmailRegistrationByID(registrationID);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public MDO.RESTDataEntities.Standard.EmailRegistration GetEmailRegistrationByUUID(string UUID)
        {
            try
            {
                RESTDLL.EmailRegistrations userDLL = new RESTDLL.EmailRegistrations(GetConnection());

                return userDLL.GetEmailRegistrationByUUID(UUID);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public MDO.RESTDataEntities.Standard.EmailRegistration GetEmailRegistrationByUserID(int userID)
        {
            try
            {
                RESTDLL.EmailRegistrations userDLL = new RESTDLL.EmailRegistrations(GetConnection());

                return userDLL.GetEmailRegistrationByUserID(userID);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public long? SaveEmailRegistration(MDO.RESTDataEntities.Standard.EmailRegistration emailRegistration)
        {
            try
            {
                RESTDLL.EmailRegistrations userDLL = new RESTDLL.EmailRegistrations(GetConnection());

                return userDLL.SaveEmailRegistration(emailRegistration);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }
    }
}
