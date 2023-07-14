using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MDO.RESTServiceRequestor.Standard
{
    public class LoginRequestor : BaseRequest
    {
        public LoginRequestor(string apiURL, string userToken, string applicationGUID)
        {
            this._apiURL = apiURL;
            this.RestClient = new RestClient(apiURL);
            base.SetHeaders(userToken, applicationGUID);
        }

        public MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.LoginResponse> Login(MDO.RESTDataEntities.Standard.LoginRequest request)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.LoginResponse>();

            try
            {
                var code = this.RestClient.SetPath("api/Login").PostRequest(JsonConvert.SerializeObject(request));

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.LoginResponse>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }
    }
}
