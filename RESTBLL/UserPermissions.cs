using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTBLL
{
    public class UserPermissions : BLLManager, IDisposable
    {

        public UserPermissions(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public UserPermissions(MySqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public List<MDO.RESTDataEntities.Standard.UserPermission> GetAllUserPermissions(bool? active)
        {
            try
            {
                RESTDLL.UserPermissions userDLL = new RESTDLL.UserPermissions(GetConnection());
                
                var all = userDLL.GetAllUserPermissions(active);

                return all;
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public List<MDO.RESTDataEntities.Standard.UserPermission> GetAllUserPermissionsByUserID(int userID)
        {
            try
            {
                RESTDLL.UserPermissions userDLL = new RESTDLL.UserPermissions(GetConnection());
                
                var all = userDLL.GetAllUserPermissionsByUserID(userID);

                return all;
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public MDO.RESTDataEntities.Standard.UserPermission GetUserPermissionByID(int userPermissionID)
        {
            try
            {
                RESTDLL.UserPermissions userDLL = new RESTDLL.UserPermissions(GetConnection());

                var all = userDLL.GetUserPermissionByID(userPermissionID);

                return all;
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public long? SaveUserPermission(MDO.RESTDataEntities.Standard.UserPermission userPermission)
        {
            try
            {
                RESTDLL.UserPermissions userDLL = new RESTDLL.UserPermissions(GetConnection());

                return userDLL.SaveUserPermission(userPermission);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public bool RemoveUserPermission(int userID, int permissionID)
        {
            try
            {
                bool isRemoved = false;

                RESTDLL.UserPermissions userPermDLL = new RESTDLL.UserPermissions(GetConnection());

                var userPermissions = userPermDLL.GetAllUserPermissionsByUserID(userID).Where(x => x.PermissionID == permissionID);

                if (userPermissions.Any()) //User has permission record
                {
                    userPermDLL.RemoveUserPermission(userID, permissionID);
                    isRemoved = true;
                }

                return isRemoved;
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return false;
        }
    }
}
