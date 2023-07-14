using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTBLL
{
    public class ExternalApplications : BLLManager, IDisposable
    {

        public ExternalApplications(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public ExternalApplications(MySqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public List<MDO.RESTDataEntities.Standard.ExternalApplication> GetAllExternalApplications()
        {
            try
            {
                RESTDLL.ExternalApplication userDLL = new RESTDLL.ExternalApplication(GetConnection());
                
                var all = userDLL.GetAllExternalApplications();

                foreach (var externalApp in all)
                {
                    externalApp.Permissions = userDLL.GetExternalApplicationPermissionsByExternalApplicationID(externalApp.ID.GetValueOrDefault());
                }

                return all;
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public MDO.RESTDataEntities.Standard.ExternalApplication GetExternalApplicationByID(int externalApplicationID)
        {
            try
            {
                RESTDLL.ExternalApplication userDLL = new RESTDLL.ExternalApplication(GetConnection());

                var all = userDLL.GetExternalApplicationByID(externalApplicationID);

                all.Permissions = userDLL.GetExternalApplicationPermissionsByExternalApplicationID(all.ID.GetValueOrDefault());

                return all;
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public long? SaveExternalApplication(MDO.RESTDataEntities.Standard.ExternalApplication externalApplication)
        {
            try
            {
                RESTDLL.ExternalApplication userDLL = new RESTDLL.ExternalApplication(GetConnection());

                return userDLL.SaveExternalApplication(externalApplication);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public long? SaveExternalApplicationPermission(MDO.RESTDataEntities.Standard.ExternalApplication.Permission externalApplication)
        {
            try
            {
                RESTDLL.ExternalApplication userDLL = new RESTDLL.ExternalApplication(GetConnection());

                return userDLL.SaveExternalApplicationPermission(externalApplication);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        #region Permissions

        public List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission> GetAllExternalApplicationPermissions()
        {
            try
            {
                RESTDLL.ExternalApplication userDLL = new RESTDLL.ExternalApplication(GetConnection());

                var all = userDLL.GetAllExternalApplicationPermissions();

                return all;
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public MDO.RESTDataEntities.Standard.ExternalApplication.Permission GetExternalApplicationPermissionByID(int externalApplicationID)
        {
            try
            {
                RESTDLL.ExternalApplication userDLL = new RESTDLL.ExternalApplication(GetConnection());

                var all = userDLL.GetExternalApplicationPermissionByID(externalApplicationID);

                return all;
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission> GetExternalApplicationPermissionsByExternalApplicationID(int externalApplicationID)
        {
            try
            {
                RESTDLL.ExternalApplication userDLL = new RESTDLL.ExternalApplication(GetConnection());

                var all = userDLL.GetExternalApplicationPermissionsByExternalApplicationID(externalApplicationID);

                return all;
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission> GetExternalApplicationPermissionsByExternalApplicationIDAndPermissionID(int externalApplicationID, int permissionID)
        {
            try
            {
                RESTDLL.ExternalApplication userDLL = new RESTDLL.ExternalApplication(GetConnection());

                var all = userDLL.GetExternalApplicationPermissionsByExternalApplicationIDAndPermissionID(externalApplicationID, permissionID);

                return all;
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        #endregion
    }
}
