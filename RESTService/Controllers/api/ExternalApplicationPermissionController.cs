using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalApplicationPermissionController : BaseController
    {
        /// <summary>
        /// Gets a list of all of the external application permissons
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission>>();

            using (var siteBAL = new RESTBLL.ExternalApplications(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var permissions = siteBAL.GetAllExternalApplicationPermissions();

                if (permissions == null)
                    return Json(response);

                var data = new List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission>();
                data = permissions;

                response.Data = data;
            }

            return Json(response);
        }

        /// <summary>
        /// Gets a list of all of the external application permissons by external application id
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllExternalApplicationPermissionsByExternalApplicationID")]
        public IActionResult GetAllExternalApplicationPermissionsByExternalApplicationID(int externalApplicationID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission>>();

            using (var siteBAL = new RESTBLL.ExternalApplications(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var permissions = siteBAL.GetExternalApplicationPermissionsByExternalApplicationID(externalApplicationID);

                if (permissions == null)
                    return Json(response);

                var data = new List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission>();
                data = permissions;

                response.Data = data;
            }

            return Json(response);
        }


        /// <summary>
        /// Gets an external application permission
        /// </summary>
        [HttpGet]
        public IActionResult Get(int externalApplicationPermissionID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.ExternalApplication.Permission>();

            using (var siteBAL = new RESTBLL.ExternalApplications(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var permissions = siteBAL.GetExternalApplicationPermissionByID(externalApplicationPermissionID);

                if (permissions == null)
                    return Json(response);

                var data = new MDO.RESTDataEntities.Standard.ExternalApplication.Permission();
                data = permissions;

                response.Data = data;
            }

            return Json(response);
        }

        /// <summary>
        /// Update or creates an external application permission. Returns the ExternalApplicationPermissionID. Only MDO admins can access this endpoint
        /// </summary>
        [HttpPut]
        public IActionResult Put(MDO.RESTDataEntities.Standard.ExternalApplication.Permission externalApplication)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<long?>("Couldn't update external application permission");

            var sessionResponse = OnlyMDOAdminEmployees(response);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            //Validation
            //if (string.IsNullOrEmpty(externalApplication.ExternalApplicationID))
            //    response.ValidationModel.Add(new MDO.RESTDataEntities.Standard.ValidationMessage("_externalApplicationID", "External Application cannot be empty", MDO.RESTDataEntities.Standard.ValidationStatus.Error));

            //if (string.IsNullOrEmpty(externalApplication.Description))
            //    response.ValidationModel.Add(new MDO.RESTDataEntities.Standard.ValidationMessage("description", "Description cannot be empty", MDO.RESTDataEntities.Standard.ValidationStatus.Error));

            if (response.ValidationModel.ValidationStatus == MDO.RESTDataEntities.Standard.ValidationStatus.Error)
            {
                return Json(response);
            }

            var session = GetSession;

            using (var siteBAL = new RESTBLL.ExternalApplications(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var foundEmployee = siteBAL.GetExternalApplicationPermissionsByExternalApplicationIDAndPermissionID(externalApplication.ExternalApplicationID, externalApplication.PermissionID).FirstOrDefault();

                long? saveID = null;

                if (foundEmployee != null)
                {
                    foundEmployee.IsRequired = externalApplication.IsRequired;
                    foundEmployee.PermissionID = externalApplication.PermissionID;
                    foundEmployee.Active = externalApplication.Active;

                    saveID = siteBAL.SaveExternalApplicationPermission(foundEmployee);
                }
                else
                {
                    saveID = siteBAL.SaveExternalApplicationPermission(externalApplication);
                }

                if (saveID == null)
                {
                    response.Status = MDO.RESTDataEntities.Standard.APIResponse<long?>.StatusEnum.Error;
                    response.Error = "Couldn't save external application permission";
                    return Json(response);
                }

                response.Data = saveID.GetValueOrDefault();
                response.Status = MDO.RESTDataEntities.Standard.APIResponse<long?>.StatusEnum.Complete;
            }

            return Json(response);
        }

        /// <summary>
        /// Update or creates a list of ExternalApplicationPermissions. Returns the last ExternalApplicationPermissionID. Perm: UserPermission.CreateUpdate
        /// </summary>
        [HttpPut("PutMultiple")]
        public IActionResult PutMultiple(List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission> externalApplicationPermissions)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<long?>("Couldn't update external application permissions");

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


            using (var siteBAL = new RESTBLL.ExternalApplications(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                foreach (var externalApplication in externalApplicationPermissions)
                {
                    var foundEmployee = siteBAL.GetExternalApplicationPermissionsByExternalApplicationIDAndPermissionID(externalApplication.ExternalApplicationID, externalApplication.PermissionID).FirstOrDefault();

                    long? saveID = null;

                    if (foundEmployee != null)
                    {
                        foundEmployee.IsRequired = externalApplication.IsRequired;
                        //foundEmployee.PermissionID = externalApplication.PermissionID;
                        foundEmployee.Active = externalApplication.Active;

                        saveID = siteBAL.SaveExternalApplicationPermission(foundEmployee);
                    }
                    else
                    {
                        saveID = siteBAL.SaveExternalApplicationPermission(externalApplication);
                    }

                    if (saveID == null)
                    {
                        response.Status = MDO.RESTDataEntities.Standard.APIResponse<long?>.StatusEnum.Error;
                        response.Error = "Couldn't save external application permission";
                        return Json(response);
                    }

                    response.Data = saveID.GetValueOrDefault();
                    response.Status = MDO.RESTDataEntities.Standard.APIResponse<long?>.StatusEnum.Complete;
                }
            }

            return Json(response);
        }
    }
}
