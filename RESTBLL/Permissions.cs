using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTBLL
{
    public class Permissions : BLLManager, IDisposable
    {

        public Permissions(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public Permissions(MySqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public List<MDO.RESTDataEntities.Standard.Permission> GetAllPermissions()
        {
            try
            {
                RESTDLL.Permissions userDLL = new RESTDLL.Permissions(GetConnection());
                
                var all = userDLL.GetAllPermissions();

                var ret = new List<MDO.RESTDataEntities.Standard.Permission>();

                foreach (var perm in all)
                {
                    if (perm.ParentPermissionID == null)
                    {
                        perm.Children = GetChildren(all, perm.ID.GetValueOrDefault());

                        ret.Add(perm);
                    }    
                }

                return ret.Distinct().OrderBy(x => x.Ordinal).ToList();
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public List<MDO.RESTDataEntities.Standard.Permission> GetAllDBPermissions()
        {
            try
            {
                RESTDLL.Permissions userDLL = new RESTDLL.Permissions(GetConnection());

                var all = userDLL.GetAllPermissions().OrderBy(x => x.Ordinal).ToList();

                return all;
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public List<MDO.RESTDataEntities.Standard.Permission> GetAllPermissionsByParentPermissionID(int parentID)
        {
            try
            {
                RESTDLL.Permissions userDLL = new RESTDLL.Permissions(GetConnection());

                var all = userDLL.GetAllPermissions();

                var children = GetChildren(all, parentID);

                return children.Distinct().OrderBy(x => x.Ordinal).ToList();
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public List<MDO.RESTDataEntities.Standard.Permission> GetChildren(List<MDO.RESTDataEntities.Standard.Permission> comments, int parentId)
        {
            return comments
                    .Where(c => c.ParentPermissionID == parentId)
                    .OrderBy(x => x.Ordinal)
                    .Select(c => new MDO.RESTDataEntities.Standard.Permission
                    {
                        ID = c.ID,
                        Name = c.Name,
                        Description = c.Description,
                        ParentPermissionID = c.ParentPermissionID,
                        CreatedDate = c.CreatedDate,
                        Children = GetChildren(comments, c.ID.GetValueOrDefault())
                    })
                    .ToList();
        }

        public MDO.RESTDataEntities.Standard.Permission GetPermissionByID(int thirdPartyUserID)
        {
            try
            {
                RESTDLL.Permissions userDLL = new RESTDLL.Permissions(GetConnection());

                var all = userDLL.GetAllPermissions();
                var children = GetChildren(all, thirdPartyUserID);

                return children.FirstOrDefault();
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public long? SavePermission(MDO.RESTDataEntities.Standard.Permission thirdParty)
        {
            try
            {
                RESTDLL.Permissions userDLL = new RESTDLL.Permissions(GetConnection());

                return userDLL.SavePermission(thirdParty);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }
    }
}
