using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTService.Controllers.api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalApplicationController : BaseController
    {
        /// <summary>
        /// Gets a list of all of the external applications
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.ExternalApplication>>();

            using (var siteBAL = new RESTBLL.ExternalApplications(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var permissions = siteBAL.GetAllExternalApplications();

                if (permissions == null)
                    return Json(response);

                var data = new List<MDO.RESTDataEntities.Standard.ExternalApplication>();
                data = permissions;

                response.Data = data;
            }

            return Json(response);
        }

        /// <summary>
        /// Gets an external application
        /// </summary>
        [HttpGet]
        public IActionResult Get(int externalApplicationID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.ExternalApplication>();

            using (var siteBAL = new RESTBLL.ExternalApplications(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var permissions = siteBAL.GetExternalApplicationByID(externalApplicationID);

                if (permissions == null)
                    return Json(response);

                var data = new MDO.RESTDataEntities.Standard.ExternalApplication();
                data = permissions;

                response.Data = data;
            }

            return Json(response);
        }

        /// <summary>
        /// Update or creates an external application. Returns the ExternalApplicationID. Only MDO admins can access this endpoint
        /// </summary>
        [HttpPut]
        public IActionResult Put(MDO.RESTDataEntities.Standard.ExternalApplication externalApplication)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<long?>("Couldn't update external application");

            var sessionResponse = OnlyMDOAdminEmployees(response);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            //Validation
            if (string.IsNullOrEmpty(externalApplication.Name))
                response.ValidationModel.Add(new MDO.RESTDataEntities.Standard.ValidationMessage("_name", "Name cannot be empty", MDO.RESTDataEntities.Standard.ValidationStatus.Error));

            if (string.IsNullOrEmpty(externalApplication.Description))
                response.ValidationModel.Add(new MDO.RESTDataEntities.Standard.ValidationMessage("description", "Description cannot be empty", MDO.RESTDataEntities.Standard.ValidationStatus.Error));

            if (response.ValidationModel.ValidationStatus == MDO.RESTDataEntities.Standard.ValidationStatus.Error)
            {
                return Json(response);
            }

            var session = GetSession;

            using (var siteBAL = new RESTBLL.ExternalApplications(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var foundEmployee = siteBAL.GetExternalApplicationByID(externalApplication.ID.GetValueOrDefault());

                long? saveID = null;

                if (foundEmployee != null)
                {
                    foundEmployee.Name = externalApplication.Name;
                    foundEmployee.Description = externalApplication.Description;

                    saveID = siteBAL.SaveExternalApplication(foundEmployee);
                }
                else
                {
                    saveID = siteBAL.SaveExternalApplication(externalApplication);
                }

                if (saveID == null)
                {
                    response.Status = MDO.RESTDataEntities.Standard.APIResponse<long?>.StatusEnum.Error;
                    response.Error = "Couldn't save external application";
                    return Json(response);
                }

                response.Data = saveID.GetValueOrDefault();
                response.Status = MDO.RESTDataEntities.Standard.APIResponse<long?>.StatusEnum.Complete;
            }

            return Json(response);
        }
    }
}
