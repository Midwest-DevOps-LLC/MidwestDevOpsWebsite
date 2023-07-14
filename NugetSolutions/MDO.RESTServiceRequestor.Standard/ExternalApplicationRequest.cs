using Newtonsoft.Json;
using RESTServiceRequestor;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTServiceRequestor.Standard
{
    public class ExternalApplicationRequest : BaseRequest
    {
        public ExternalApplicationRequest(string apiURL, string userToken, string applicationGUID)
        {
            this._apiURL = apiURL;
            this.RestClient = new RestClient(apiURL);
            base.SetHeaders(userToken, applicationGUID);
        }

        public MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.ExternalApplication> Get(int externalApplicationID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.ExternalApplication>();

            try
            {
                var code = this.RestClient.SetPath("api/ExternalApplication").GetRequest(new Dictionary<string, string>() { { "externalApplicationID", externalApplicationID.ToString()} });
                //var code = this.RestClient.SetPath("api/Employee").PostRequest(JsonConvert.SerializeObject(request));

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.ExternalApplication>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.ExternalApplication>> GetAll()
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.ExternalApplication>>();

            try
            {
                var code = this.RestClient.SetPath("api/ExternalApplication/GetAll").GetRequest();
                //var code = this.RestClient.SetPath("api/Employee").PostRequest(JsonConvert.SerializeObject(request));

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.ExternalApplication>>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<long?> Put(MDO.RESTDataEntities.Standard.ExternalApplication employee)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<long?>();

            try
            {
                var requestData = JsonConvert.SerializeObject(employee);

                var code = this.RestClient.SetPath("api/ExternalApplication").PutRequest(requestData);
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
