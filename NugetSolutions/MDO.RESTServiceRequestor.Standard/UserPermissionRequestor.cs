using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTServiceRequestor.Standard
{
    public class UserPermissionRequestor : BaseRequest
    {
        public UserPermissionRequestor(string apiURL, string userToken, string applicationGUID)
        {
            this._apiURL = apiURL;
            this.RestClient = new RestClient(apiURL);
            base.SetHeaders(userToken, applicationGUID);
        }

        public MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.UserPermission>> GetAll()
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.UserPermission>>();

            try
            {
                var code = this.RestClient.SetPath("api/UserPermission/GetAll").GetRequest();

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.UserPermission>>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.UserPermission>> GetAllUserPermissionsByUserID(int userID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.UserPermission>>();

            try
            {
                var code = this.RestClient.SetPath("api/UserPermission/GetAllUserPermissionsByUserID").GetRequest(new Dictionary<string, string>() { { "userID", userID.ToString() } });
                //var code = this.RestClient.SetPath("api/Employee").PostRequest(JsonConvert.SerializeObject(request));

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.UserPermission>>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.UserPermission> Get(int userPermissionID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.UserPermission>();

            try
            {
                var code = this.RestClient.SetPath("api/UserPermission").GetRequest(new Dictionary<string, string>() { { "userPermissionID", userPermissionID.ToString() } });
                //var code = this.RestClient.SetPath("api/Employee").PostRequest(JsonConvert.SerializeObject(request));

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.UserPermission>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.UserPermission> UserHasPermission(int userID, int permissionID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.UserPermission>();

            try
            {
                var code = this.RestClient.SetPath("api/UserPermission/UserHasPermission").GetRequest(new Dictionary<string, string>() { { "userID", userID.ToString() }, { "permissionID", permissionID.ToString() } });
                //var code = this.RestClient.SetPath("api/Employee").PostRequest(JsonConvert.SerializeObject(request));

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.UserPermission>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<long?> Put(MDO.RESTDataEntities.Standard.UserPermission userPermission)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<long?>();

            try
            {
                var requestData = JsonConvert.SerializeObject(userPermission);

                var code = this.RestClient.SetPath("api/UserPermission").PutRequest(requestData);
                //var code = this.RestClient.SetPath("api/Employee").PostRequest(JsonConvert.SerializeObject(request));

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<long?>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<long?> PutMultiple(List<MDO.RESTDataEntities.Standard.UserPermission> userPermissions)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<long?>();

            try
            {
                var requestData = JsonConvert.SerializeObject(userPermissions);

                var code = this.RestClient.SetPath("api/UserPermission/PutMultiple").PutRequest(requestData);
                //var code = this.RestClient.SetPath("api/Employee").PostRequest(JsonConvert.SerializeObject(request));

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<long?>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<long?> Put(MDO.RESTDataEntities.Standard.DeleteUserPermissionRequest userPermission)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<long?>();

            try
            {
                var requestData = JsonConvert.SerializeObject(userPermission);

                var code = this.RestClient.SetPath("api/UserPermission").DeleteRequest(requestData);
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
