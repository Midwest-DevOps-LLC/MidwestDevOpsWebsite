using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserPermissionController : BaseController
    {
        public UserPermissionController(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Gets a list of all of the user permissions. Perm: UserPermission.ViewAll
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.UserPermission>>();

            var sessionResponse = OnlyPermission(response, MDO.RESTDataEntities.Standard.Permission.PermissionLookup.UserPermission.ViewAll);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            using (var siteBAL = new RESTBLL.UserPermissions(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var permissions = siteBAL.GetAllUserPermissions(null);

                if (permissions == null)
                    return Json(response);

                var data = new List<MDO.RESTDataEntities.Standard.UserPermission>();
                data = permissions;

                response.Data = data;
            }

            return Json(response);
        }

        /// <summary>
        /// Gets a list of all of the user permissions by userID
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllUserPermissionsByUserID")]
        public IActionResult GetAllUserPermissionsByUserID(int userID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.UserPermission>>();

            //var sessionResponse = OnlyPermission(response, MDO.RESTDataEntities.Standard.Permission.PermissionLookup.UserPermission.ViewAll);

            //if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
            //    return Json(response);

            using (var siteBAL = new RESTBLL.UserPermissions(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var permBAL = new RESTBLL.Permissions(siteBAL.GetConnection());

                var userPermissions = siteBAL.GetAllUserPermissionsByUserID(userID);
                var allPerms = permBAL.GetAllDBPermissions();

                foreach (var perm in allPerms)
                {
                    if (perm.UserAlwaysHas)
                    {
                        if (userPermissions.Where(x => x.PermissionID == perm.ID).Any() == false) //No default perm record found
                        {
                            userPermissions.Add(new MDO.RESTDataEntities.Standard.UserPermission()
                            {
                                Active = true,
                                PermissionID = perm.ID.GetValueOrDefault(),
                                UserID = userID
                            });
                        }
                        else
                        {
                            foreach (var defaultPerm in userPermissions.Where(x => x.PermissionID == perm.ID).ToList())
                            {
                                defaultPerm.Active = true;
                            }
                        }
                    }
                }

                if (userPermissions == null)
                    return Json(response);

                var data = new List<MDO.RESTDataEntities.Standard.UserPermission>();
                data = userPermissions.Distinct().ToList();

                response.Data = data;
            }

            return Json(response);
        }

        /// <summary>
        /// Gets a user permission
        /// </summary>
        [HttpGet]
        public IActionResult Get(int userPermissionID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.UserPermission>();

            //var sessionResponse = OnlySessions(response);

            //if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
            //    return Json(response);

            //var userSession = GetSession;

            using (var siteBAL = new RESTBLL.UserPermissions(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var permissions = siteBAL.GetUserPermissionByID(userPermissionID);

                if (permissions == null)
                    return Json(response);

                //var permResponse = OnlyPermission(response, MDO.RESTDataEntities.Standard.Permission.PermissionLookup.UserPermission.ViewAll);

                //if (permissions == null && permResponse.IsSessionValid == true)
                //    return Json(response);
                //else if (permissions == null && permResponse.IsSessionValid == false)
                //    return Json(permResponse);

                //if (permissions.UserID != userSession.UserID)
                //{
                //    if (permResponse.IsSessionValid == false) //If session doesn't has access
                //    {
                //        return Json(response);
                //    }
                //}

                var data = new MDO.RESTDataEntities.Standard.UserPermission();
                data = permissions;

                response.Data = data;
            }

            return Json(response);
        }

        /// <summary>
        /// Returns true if the user has the permission
        /// </summary>
        [HttpGet("UserHasPermission")]
        public IActionResult UserHasPermission(int userID, int permissionID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<bool>();

            using (var siteBAL = new RESTBLL.UserPermissions(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var permBAL = new RESTBLL.Permissions(siteBAL.GetConnection());

                var userPermissions = siteBAL.GetAllUserPermissionsByUserID(userID);

                var allPerms = permBAL.GetAllDBPermissions();

                foreach (var perm in allPerms)
                {
                    if (perm.UserAlwaysHas)
                    {
                        if (userPermissions.Where(x => x.PermissionID == perm.ID).Any() == false) //No default perm record found
                        {
                            userPermissions.Add(new MDO.RESTDataEntities.Standard.UserPermission()
                            {
                                Active = true,
                                PermissionID = perm.ID.GetValueOrDefault(),
                                UserID = userID
                            });
                        }
                        else
                        {
                            foreach (var defaultPerm in userPermissions.Where(x => x.PermissionID == perm.ID).ToList())
                            {
                                defaultPerm.Active = true;
                            }
                        }
                    }
                }

                if (userPermissions == null)
                {
                    response.Data = false;
                }
                else {
                    var perm = userPermissions.Where(x => x.PermissionID == permissionID).FirstOrDefault();

                    if (perm == null)
                    {
                        response.Data = false;
                    }
                    else
                    {
                        response.Data = true;
                    }
                }
            }

            return Json(response);
        }

        /// <summary>
        /// Update or creates a list of userpermissions. Returns the last UserPermissionID. Perm: UserPermission.CreateUpdate
        /// </summary>
        [HttpPut("PutMultiple")]
        public IActionResult PutMultiple(List<MDO.RESTDataEntities.Standard.UserPermission> userPermissions)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<long?>("Couldn't update user permission");

            var sessionResponse = OnlyPermission(response, MDO.RESTDataEntities.Standard.Permission.PermissionLookup.UserPermission.CreateUpdate);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            //Validation
            //if (string.IsNullOrEmpty(userPermission.Name))
            //    response.ValidationModel.Add(new MDO.RESTDataEntities.Standard.ValidationMessage("_name", "Name cannot be empty", MDO.RESTDataEntities.Standard.ValidationStatus.Error));

            //if (string.IsNullOrEmpty(userPermission.Description))
            //    response.ValidationModel.Add(new MDO.RESTDataEntities.Standard.ValidationMessage("description", "Description cannot be empty", MDO.RESTDataEntities.Standard.ValidationStatus.Error));

            if (response.ValidationModel.ValidationStatus == MDO.RESTDataEntities.Standard.ValidationStatus.Error)
            {
                return Json(response);
            }

            var session = GetSession;

            using (var siteBAL = new RESTBLL.UserPermissions(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                foreach (var userPermission in userPermissions)
                {
                    var foundEmployee = siteBAL.GetAllUserPermissionsByUserID(userPermission.UserID).Where(x => x.PermissionID == userPermission.PermissionID).FirstOrDefault();

                    long? saveID = null;

                    if (foundEmployee != null)
                    {
                        foundEmployee.UserID = userPermission.UserID;
                        foundEmployee.PermissionID = userPermission.PermissionID;
                        foundEmployee.Active = userPermission.Active;

                        saveID = siteBAL.SaveUserPermission(foundEmployee);
                    }
                    else
                    {
                        saveID = siteBAL.SaveUserPermission(userPermission);
                    }

                    if (saveID == null)
                    {
                        response.Status = MDO.RESTDataEntities.Standard.APIResponse<long?>.StatusEnum.Error;
                        response.Error = "Couldn't save user permission";
                        return Json(response);
                    }

                    response.Data = saveID.GetValueOrDefault();
                    response.Status = MDO.RESTDataEntities.Standard.APIResponse<long?>.StatusEnum.Complete;
                }
            }

            return Json(response);
        }

        /// <summary>
        /// Update or creates a userpermission. Returns the UserPermissionID. Perm: UserPermission.CreateUpdate
        /// </summary>
        [HttpPut]
        public IActionResult Put(MDO.RESTDataEntities.Standard.UserPermission userPermission)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<long?>("Couldn't update user permission");

            var sessionResponse = OnlyPermission(response, MDO.RESTDataEntities.Standard.Permission.PermissionLookup.UserPermission.CreateUpdate);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            //Validation
            //if (string.IsNullOrEmpty(userPermission.Name))
            //    response.ValidationModel.Add(new MDO.RESTDataEntities.Standard.ValidationMessage("_name", "Name cannot be empty", MDO.RESTDataEntities.Standard.ValidationStatus.Error));

            //if (string.IsNullOrEmpty(userPermission.Description))
            //    response.ValidationModel.Add(new MDO.RESTDataEntities.Standard.ValidationMessage("description", "Description cannot be empty", MDO.RESTDataEntities.Standard.ValidationStatus.Error));

            if (response.ValidationModel.ValidationStatus == MDO.RESTDataEntities.Standard.ValidationStatus.Error)
            {
                return Json(response);
            }

            var session = GetSession;

            using (var siteBAL = new RESTBLL.UserPermissions(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var foundEmployee = siteBAL.GetAllUserPermissionsByUserID(userPermission.UserID).Where(x => x.PermissionID == userPermission.PermissionID).FirstOrDefault();

                long? saveID = null;

                if (foundEmployee != null)
                {
                    foundEmployee.UserID = userPermission.UserID;
                    foundEmployee.PermissionID = userPermission.PermissionID;
                    foundEmployee.Active = userPermission.Active;

                    saveID = siteBAL.SaveUserPermission(foundEmployee);
                }
                else
                {
                    saveID = siteBAL.SaveUserPermission(userPermission);
                }

                if (saveID == null)
                {
                    response.Status = MDO.RESTDataEntities.Standard.APIResponse<long?>.StatusEnum.Error;
                    response.Error = "Couldn't save user permission";
                    return Json(response);
                }

                response.Data = saveID.GetValueOrDefault();
                response.Status = MDO.RESTDataEntities.Standard.APIResponse<long?>.StatusEnum.Complete;
            }

            return Json(response);
        }

        /// <summary>
        /// Tries to remove a permission from a user. Returns false if the permission wasn't added to the user. Perm: UserPermission.Remove
        /// </summary>
        [HttpDelete("RemoveUserPermission")]
        public IActionResult RemoveUserPermission(MDO.RESTDataEntities.Standard.DeleteUserPermissionRequest request)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<bool>();

            var sessionResponse = OnlyPermission(response, MDO.RESTDataEntities.Standard.Permission.PermissionLookup.UserPermission.Remove);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            using (var siteBLL = new RESTBLL.UserPermissions(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var s = siteBLL.RemoveUserPermission(request.UserID, request.PermissionID);
                response.Data = s;
                response.Status = MDO.RESTDataEntities.Standard.APIResponse<bool>.StatusEnum.Complete;
            }

            if (response.Data == false)
            {
                response.Status = MDO.RESTDataEntities.Standard.APIResponse<bool>.StatusEnum.Error;
                response.Error = "Couldn't remove user from permission: " + request.PermissionID + ". It's possible if the user never had access to the permission";
            }

            return Json(response);
        }
    }
}
