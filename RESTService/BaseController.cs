using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTService
{
    public class BaseController : Controller
    {
        internal IConfiguration _configuration;

        public BaseController()
        {
        }

        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetAuthorizationHeader
        {
            get
            {
                if (Request.Headers.ContainsKey("Authorization"))
                {
                    if (string.IsNullOrEmpty(Request.Headers["Authorization"]) == false)
                    {
                        return Request.Headers["Authorization"];
                    }
                }

                return null;
            }
        }

        public string GetX_Application_GUIDHeader
        {
            get
            {
                if (Request.Headers.ContainsKey("X-Application-GUID"))
                {
                    if (string.IsNullOrEmpty(Request.Headers["X-Application-GUID"]) == false)
                    {
                        return Request.Headers["X-Application-GUID"];
                    }
                }

                return null;
            }
        }

        public MDO.RESTDataEntities.Standard.UserSession GetSession
        {
            get
            {
                var auth = GetAuthorizationHeader;

                using (var userSessionBLL = new RESTBLL.UserSessions(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
                {
                    return userSessionBLL.Verify(auth);
                }
            }
        }

        private List<MDO.RESTDataEntities.Standard.UserPermission> _GetUserPermissions { get; set; } = new List<MDO.RESTDataEntities.Standard.UserPermission>();
        public List<MDO.RESTDataEntities.Standard.UserPermission> GetUserPermissions
        {
            get
            {
                if (_GetUserPermissions == null || _GetUserPermissions.Count < 1)
                {
                    var auth = GetAuthorizationHeader;

                    using (var userSessionBLL = new RESTBLL.UserPermissions(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
                    {
                        _GetUserPermissions =  userSessionBLL.GetAllUserPermissionsByUserID(GetSession.UserID);
                    }
                }

                return _GetUserPermissions;
            }
        }

        public bool IsMDOEmployeeSession
        {
            get
            {
                var session = GetSession;

                if (session == null)
                    return false;

                using (var employeeBLL = new RESTBLL.Employees(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
                {
                    var employee = employeeBLL.GetEmployeeByUserID(session.UserID);

                    if (employee == null)
                        return false;
                }

                return true;
            }
        }

        public bool IsMDOAdminEmployeeSession
        {
            get
            {
                var session = GetSession;

                if (session == null)
                    return false;

                using (var employeeBLL = new RESTBLL.Employees(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
                {
                    var employee = employeeBLL.GetEmployeeByUserID(session.UserID);

                    if (employee == null)
                        return false;

                    if (employee.IsAdmin == false)
                        return false;
                }

                return true;
            }
        }

        private List<MDO.RESTDataEntities.Standard.Permission> _allDBPermissions { get; set; } = null;
        public List<MDO.RESTDataEntities.Standard.Permission> AllDBPermissions
        {
            get
            {
                if (_allDBPermissions == null)
                {
                    using (var employeeBLL = new RESTBLL.Permissions(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
                    {
                        _allDBPermissions = employeeBLL.GetAllDBPermissions();
                    }
                }

                return _allDBPermissions;
            }
        }

        public MDO.RESTDataEntities.Standard.APIResponse<T> OnlySessions<T>(MDO.RESTDataEntities.Standard.APIResponse<T> response)
        {
            if (GetSession == null)
            {
                response.Error = "Session is not valid";
                response.IsSessionValid = false;
                response.Status = MDO.RESTDataEntities.Standard.APIResponse<T>.StatusEnum.Error;
            }
            else
            {
                response.IsSessionValid = true;
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<T> OnlyMDOEmployees<T>(MDO.RESTDataEntities.Standard.APIResponse<T> response)
        {
            response = OnlySessions(response);

            if (response.IsSessionValid == false)
                return response;

            if (IsMDOEmployeeSession == false)
            {
                response.Error = "User isn't a Midwest DevOps employee";
                response.IsSessionValid = false;
                response.Status = MDO.RESTDataEntities.Standard.APIResponse<T>.StatusEnum.Error;
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<T> OnlyMDOAdminEmployees<T>(MDO.RESTDataEntities.Standard.APIResponse<T> response)
        {
            response = OnlySessions(response);

            if (response.IsSessionValid == false)
                return response;

            if (IsMDOAdminEmployeeSession == false)
            {
                response.Error = "User isn't a Midwest DevOps admin employee";
                response.IsSessionValid = false;
                response.Status = MDO.RESTDataEntities.Standard.APIResponse<T>.StatusEnum.Error;
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<T> OnlyPermission<T>(MDO.RESTDataEntities.Standard.APIResponse<T> response, int permissionLID)
        {
            response = OnlySessions(response);

            if (response.IsSessionValid == false)
                return response;

            var userPerms = GetUserPermissions;

            var perm = AllDBPermissions.Where(x => x.ID == permissionLID).FirstOrDefault();

            if (perm != null && perm.UserAlwaysHas)
            {
                return response;
            }

            if (userPerms.Select(x => x.PermissionID).Contains(permissionLID) == false || userPerms.Where(x => x.PermissionID == permissionLID).Any(x => x.Active == false))
            {
                response.Error = "User doesn't have the correct permission or the requested data isn't there";
                response.IsSessionValid = false;
                response.Status = MDO.RESTDataEntities.Standard.APIResponse<T>.StatusEnum.Error;
            }

            return response;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public bool CheckIfSessionHasPermission(int permissionLID)
        {
            var userPerms = GetUserPermissions;

            if (userPerms.Select(x => x.PermissionID).Contains(permissionLID) == false)
            {
                return false;
            }

            return true;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
    }
}
