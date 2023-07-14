using Newtonsoft.Json;
using RESTServiceRequestor;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTServiceRequestor.Standard
{
    public class PermissionRequestor : BaseRequest
    {
        public PermissionRequestor(string apiURL, string userToken, string applicationGUID)
        {
            this._apiURL = apiURL;
            this.RestClient = new RestClient(apiURL);
            base.SetHeaders(userToken, applicationGUID);
        }

        public MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Permission>> GetAllPermissions()
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Permission>>();

            try
            {
                var code = this.RestClient.SetPath("api/Permission/GetAllPermissions").GetRequest();

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Permission>>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Permission>> GetAllDBPermissions()
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Permission>>();

            try
            {
                var code = this.RestClient.SetPath("api/Permission/GetAllDBPermissions").GetRequest();

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Permission>>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Permission>> GetAllPermissionsByParentPermissionID(int parentID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Permission>>();

            try
            {
                var code = this.RestClient.SetPath("api/Permission/GetAllPermissionsByParentPermissionID").GetRequest(new Dictionary<string, string>() { { "parentID", parentID.ToString() } });

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Permission>>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }
    }
}
