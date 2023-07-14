using Newtonsoft.Json;
using RESTServiceRequestor;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTServiceRequestor.Standard
{
    public class EmployeeRequest : BaseRequest
    {
        public EmployeeRequest(string apiURL, string userToken, string applicationGUID)
        {
            this._apiURL = apiURL;
            this.RestClient = new RestClient(apiURL);
            base.SetHeaders(userToken, applicationGUID);
        }

        public MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.Employee> Get(int employeeID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.Employee>();

            try
            {
                var code = this.RestClient.SetPath("api/Employee").GetRequest(new Dictionary<string, string>() { { "employeeID", employeeID.ToString()} });
                //var code = this.RestClient.SetPath("api/Employee").PostRequest(JsonConvert.SerializeObject(request));

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.Employee>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Employee>> GetAll(bool? active)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Employee>>();

            try
            {
                var code = this.RestClient.SetPath("api/Employee/GetAll").GetRequest(new Dictionary<string, string>() { { "active", active.ToString() } });
                //var code = this.RestClient.SetPath("api/Employee").PostRequest(JsonConvert.SerializeObject(request));

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Employee>>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<long?> Put(MDO.RESTDataEntities.Standard.Employee employee)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<long?>();

            try
            {
                var requestData = JsonConvert.SerializeObject(employee);

                var code = this.RestClient.SetPath("api/Employee").PutRequest(requestData);
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
