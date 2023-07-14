using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using MDO.Utility.Standard;
using System.Linq;

namespace RESTBLL
{
    public class Sites : BLLManager, IDisposable
    {
        public const int MINUTES_TIL_TIMEOUT = 30;

        public Sites(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public Sites(MySqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public bool UserHasAccessToSite(string siteGUID, int userID)
        {
            try
            {
                var site = GetSiteByGUID(siteGUID);

                if (site == null)
                {
                    //response.Error = "Couldn't find site by GUID";
                    return false;
                }

                if (site.AllowAllUsers) //If the site is set to 'AllowAllUsers' then everyone has access to log into it
                {
                    AddUser(site.SiteID.GetValueOrDefault(), userID);
                    return true;
                }
                else
                {
                    var userSiteForApp = GetAllSitesByUserID(userID, true).Where(x => x.GUID == siteGUID);

                    if (userSiteForApp.Any() == false) //User doesn't have a usersite record
                    {
                        //response.Error = "User doesn't have access to this site";
                        return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return false;
        }

        public List<MDO.RESTDataEntities.Standard.Site> GetAllSites(bool? active)
        {
            try
            {
                RESTDLL.Sites userDLL = new RESTDLL.Sites(GetConnection());

                return userDLL.GetAllSites(active);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public MDO.RESTDataEntities.Standard.Site GetSiteByID(int siteID)
        {
            try
            {
                RESTDLL.Sites userDLL = new RESTDLL.Sites(GetConnection());

                return userDLL.GetSiteByID(siteID);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public MDO.RESTDataEntities.Standard.Site GetSiteByGUID(string guid)
        {
            try
            {
                RESTDLL.Sites userDLL = new RESTDLL.Sites(GetConnection());

                return userDLL.GetSiteByGUID(guid);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public List<MDO.RESTDataEntities.Standard.Site> GetAllSitesByUserID(int userID, bool? active)
        {
            try
            {
                RESTDLL.Sites userDLL = new RESTDLL.Sites(GetConnection());

                return userDLL.GetAllSitesByUserID(userID, active);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return new List<MDO.RESTDataEntities.Standard.Site>();
        }

        public long? SaveSite(MDO.RESTDataEntities.Standard.Site userSession)
        {
            try
            {
                RESTDLL.Sites userDLL = new RESTDLL.Sites(GetConnection());

                return userDLL.SaveSite(userSession);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public long? AddUser(int siteID, int userID)
        {
            try
            {
                RESTDLL.Sites userDLL = new RESTDLL.Sites(GetConnection());

                var userSites = GetAllSitesByUserID(userID, null).Where(x => x.SiteID == siteID);

                if (userSites.Any()) //User already has site access
                {
                    var siteUser = userDLL.GetSiteUserBySiteIDAndUserID(siteID, userID);

                    return siteUser.SiteUserID;
                }

                return userDLL.AddUser(siteID, userID);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public bool RemoveUser(int siteID, int userID)
        {
            try
            {
                bool isRemoved = false;

                RESTDLL.Sites userDLL = new RESTDLL.Sites(GetConnection());

                var userSites = GetAllSitesByUserID(userID, null).Where(x => x.SiteID == siteID);

                if (userSites.Any()) //User has site access
                {
                    userDLL.RemoveUser(siteID, userID);
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
