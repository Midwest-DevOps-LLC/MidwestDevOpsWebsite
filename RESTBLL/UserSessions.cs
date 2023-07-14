using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using MDO.Utility.Standard;
using System.Linq;

namespace RESTBLL
{
    public class UserSessions : BLLManager, IDisposable
    {
        public const int MINUTES_TIL_TIMEOUT = 30;

        public UserSessions(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public UserSessions(MySqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public MDO.RESTDataEntities.Standard.UserSession Verify(string authKey)
        {
            MDO.RESTDataEntities.Standard.UserSession ret = null;

            if (string.IsNullOrEmpty(authKey))
                return null;

            var session = GetUserSessionByToken(authKey);

            if (session == null)
                return null;

            if (session.ModifiedDate.AddMinutes(RESTBLL.UserSessions.MINUTES_TIL_TIMEOUT) <= DateTime.UtcNow) //Too old session
            {
                ret = null;
            }
            else //Inside timeout time and it will now update it
            {
                ret = SaveOrUpdateSession(session);
            }

            return ret;
        }

        public List<MDO.RESTDataEntities.Standard.UserSession> GetAllUserSessions()
        {
            try
            {
                RESTDLL.UserSessions userDLL = new RESTDLL.UserSessions(GetConnection());

                return userDLL.GetAllUserSessions();
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public MDO.RESTDataEntities.Standard.UserSession GetUserSessionByID(int sessionID)
        {
            try
            {
                RESTDLL.UserSessions userDLL = new RESTDLL.UserSessions(GetConnection());

                return userDLL.GetUserSessionByID(sessionID);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public MDO.RESTDataEntities.Standard.UserSession GetUserSessionByToken(string token)
        {
            try
            {
                RESTDLL.UserSessions userDLL = new RESTDLL.UserSessions(GetConnection());

                return userDLL.GetUserSessionByToken(token);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public List<MDO.RESTDataEntities.Standard.UserSession> GetAllUserSessionByUserID(int userID)
        {
            try
            {
                RESTDLL.UserSessions userDLL = new RESTDLL.UserSessions(GetConnection());

                return userDLL.GetAllUserSessionByUserID(userID);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return new List<MDO.RESTDataEntities.Standard.UserSession>();
        }

        public MDO.RESTDataEntities.Standard.UserSession SaveOrUpdateSession(MDO.RESTDataEntities.Standard.UserSession userSession)
        {
            RESTDLL.UserSessions userSessionDLL = new RESTDLL.UserSessions(GetConnection());

            var userDLL = new RESTDLL.Users(userSessionDLL.GetConnection());
            var employeeDLL = new RESTDLL.Employees(userSessionDLL.GetConnection());

            var employee = employeeDLL.GetEmployeeByUserID(userSession.UserID);

            if (employee != null)
            {
                userSession.IsMDOEmployee = true;
                userSession.IsMDOAdmin = employee.IsAdmin;
            }

            var userSessions = GetAllUserSessionByUserID(userSession.UserID);

            var latestUserSession = userSessions.OrderByDescending(x => x.ModifiedDate).FirstOrDefault();

            if (latestUserSession != null)
            {
                if (DateTime.UtcNow > latestUserSession.ModifiedDate.AddMinutes(MINUTES_TIL_TIMEOUT)) //Need new session
                {
                    userSession.ModifiedDate = DateTime.UtcNow.AddMinutes(MINUTES_TIL_TIMEOUT);

                    SaveUserSession(userSession);

                    return userSession;
                }

                latestUserSession.ModifiedDate = DateTime.UtcNow.AddMinutes(MINUTES_TIL_TIMEOUT);

                if (employee != null)
                {
                    latestUserSession.IsMDOEmployee = true;
                    latestUserSession.IsMDOAdmin = employee.IsAdmin;
                }

                SaveUserSession(latestUserSession);

                return latestUserSession;
            }

            userSession.ModifiedDate = DateTime.UtcNow.AddMinutes(MINUTES_TIL_TIMEOUT);

            SaveUserSession(userSession);

            return userSession;
        }

        public long? SaveUserSession(MDO.RESTDataEntities.Standard.UserSession userSession) //Get all user's sessions. If any of them are within timeout range then just do an update. otherwise create a new session
        {
            try
            {
                RESTDLL.UserSessions userDLL = new RESTDLL.UserSessions(GetConnection());

                return userDLL.SaveUserSession(userSession);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }
    }
}
