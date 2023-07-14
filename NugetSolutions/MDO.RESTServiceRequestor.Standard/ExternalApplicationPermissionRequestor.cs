using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTServiceRequestor.Standard
{
    public class ExternalApplicationPermissionRequestor : BaseRequest
    {
        public ExternalApplicationPermissionRequestor(string apiURL, string userToken, string applicationGUID)
        {
            this._apiURL = apiURL;
            this.RestClient = new RestClient(apiURL);
            base.SetHeaders(userToken, applicationGUID);
        }

        public MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission>> GetAllExternalApplicationPermissions()
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission>>();

            try
            {
                var code = this.RestClient.SetPath("api/ExternalApplicationPermission/GetAll").GetRequest();

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission>>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.ExternalApplication.Permission> GetExternalApplicationPermissionsByExternalApplicationPermissionID(int externalApplicationPermissionID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.ExternalApplication.Permission>();

            try
            {
                var code = this.RestClient.SetPath("api/ExternalApplicationPermission").GetRequest(new Dictionary<string, string>() { { "externalApplicationPermissionID", externalApplicationPermissionID.ToString() } });

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.ExternalApplication.Permission>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission>> GetAllExternalApplicationPermissionsByExternalApplicationID(int externalApplicationID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission>>();

            try
            {
                var code = this.RestClient.SetPath("api/ExternalApplicationPermission/GetAllExternalApplicationPermissionsByExternalApplicationID").GetRequest(new Dictionary<string, string>() { { "externalApplicationID", externalApplicationID.ToString() } });

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission>>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<long?> Put(MDO.RESTDataEntities.Standard.ExternalApplication.Permission externalApplicationPermission)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<long?>();

            try
            {
                var requestData = JsonConvert.SerializeObject(externalApplicationPermission);

                var code = this.RestClient.SetPath("api/ExternalApplicationPermission").PutRequest(requestData);
                //var code = this.RestClient.SetPath("api/Employee").PostRequest(JsonConvert.SerializeObject(request));

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<long?>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<long?> PutMultiple(List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission> externalApplicationPermissions)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<long?>();

            try
            {
                var requestData = JsonConvert.SerializeObject(externalApplicationPermissions);

                var code = this.RestClient.SetPath("api/ExternalApplicationPermission/PutMultiple").PutRequest(requestData);
                //var code = this.RestClient.SetPath("api/Employee").PostRequest(JsonConvert.SerializeObject(request));

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<long?>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }
    }
}
